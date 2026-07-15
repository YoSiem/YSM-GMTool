using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Serilog;

namespace App.Desktop.Infrastructure;

/// <summary>
/// Resolves entity icon keys to letterboxed square <see cref="Bitmap"/>s from a configured root folder:
/// rooted/extension/extension-probe path resolution, no-upscale centered letterboxing, and a negative
/// cache (failed lookups cached as null). Cache key is <c>"{size}:{trimmedKey}"</c>.
/// </summary>
public static class IconCache
{
    private static readonly string[] IconExtensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];
    private static readonly object Gate = new();
    private static readonly Dictionary<string, Bitmap?> Cache = new(StringComparer.OrdinalIgnoreCase);

    private static bool _enabled;
    private static string? _rootPath;

    /// <summary>Enable/disable icon resolution and set the root folder. Clears the cache when config changes.</summary>
    public static void Configure(bool enabled, string? rootPath)
    {
        var normalized = string.IsNullOrWhiteSpace(rootPath) ? null : rootPath.Trim();

        bool changed;
        lock (Gate)
        {
            changed = _enabled != enabled
                || !string.Equals(_rootPath, normalized, StringComparison.OrdinalIgnoreCase);

            _enabled = enabled;
            _rootPath = normalized;

            if (changed)
            {
                Cache.Clear();
            }
        }

        Log.Debug(
            "IconCache.Configure: enabled={Enabled} root={Root} rootExists={RootExists} changed={Changed}",
            enabled,
            normalized ?? "<null>",
            normalized is not null && Directory.Exists(normalized),
            changed);
    }

    /// <summary>Resolve a key to a square bitmap of <paramref name="iconSize"/>, or null if unavailable.</summary>
    public static Bitmap? Resolve(string? iconKey, int iconSize)
    {
        bool enabled;
        string? root;
        lock (Gate)
        {
            enabled = _enabled;
            root = _rootPath;
        }

        if (!enabled || string.IsNullOrWhiteSpace(root) || string.IsNullOrWhiteSpace(iconKey))
        {
            Log.Debug(
                "IconCache.Resolve: skipped (enabled={Enabled} hasRoot={HasRoot} key={Key}).",
                enabled,
                !string.IsNullOrWhiteSpace(root),
                iconKey ?? "<null>");
            return null;
        }

        var normalizedKey = iconKey.Trim();
        var size = Math.Max(8, iconSize);
        var cacheKey = $"{size}:{normalizedKey}";

        lock (Gate)
        {
            if (Cache.TryGetValue(cacheKey, out var cached))
            {
                Log.Debug(
                    "IconCache.Resolve: cache hit key={Key} size={Size} bitmap={HasBitmap}.",
                    normalizedKey,
                    size,
                    cached is not null);
                return cached;
            }
        }

        var result = LoadAndLetterbox(root, normalizedKey, size);

        lock (Gate)
        {
            Cache[cacheKey] = result;
        }

        return result;
    }

    private static Bitmap? LoadAndLetterbox(string root, string iconKey, int size)
    {
        var path = ResolveIconFilePath(root, iconKey);
        if (string.IsNullOrWhiteSpace(path))
        {
            Log.Debug(
                "IconCache.Resolve: file NOT FOUND for key={Key} under root={Root} (size={Size}).",
                iconKey,
                root,
                size);
            return null;
        }

        try
        {
            using var source = new Bitmap(path);
            var bitmap = ResizeWithLetterboxing(source, size);
            Log.Debug(
                "IconCache.Resolve: loaded key={Key} path={Path} src={SrcW}x{SrcH} -> size={Size}.",
                iconKey,
                path,
                source.PixelSize.Width,
                source.PixelSize.Height,
                size);
            return bitmap;
        }
        catch (Exception ex)
        {
            Log.Debug(
                ex,
                "IconCache.Resolve: bitmap load FAILED for key={Key} path={Path}.",
                iconKey,
                path);
            return null;
        }
    }

    private static string? ResolveIconFilePath(string root, string iconKey)
    {
        if (!Directory.Exists(root))
        {
            Log.Debug("IconCache.Resolve: root folder does not exist root={Root}.", root);
            return null;
        }

        if (Path.IsPathRooted(iconKey))
        {
            return File.Exists(iconKey) ? iconKey : null;
        }

        if (Path.HasExtension(iconKey))
        {
            var direct = Path.Combine(root, iconKey);
            return File.Exists(direct) ? direct : null;
        }

        foreach (var extension in IconExtensions)
        {
            var candidate = Path.Combine(root, iconKey + extension);
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static Bitmap ResizeWithLetterboxing(Bitmap source, int iconSize)
    {
        var fit = GetFitSize(source.PixelSize.Width, source.PixelSize.Height, iconSize);

        var target = new RenderTargetBitmap(new PixelSize(iconSize, iconSize), new Vector(96, 96));
        using (var ctx = target.CreateDrawingContext())
        {
            var x = (iconSize - fit.Width) / 2.0;
            var y = (iconSize - fit.Height) / 2.0;
            ctx.DrawImage(
                source,
                new Rect(0, 0, source.PixelSize.Width, source.PixelSize.Height),
                new Rect(x, y, fit.Width, fit.Height));
        }

        return target;
    }

    private static PixelSize GetFitSize(int sourceWidth, int sourceHeight, int target)
    {
        if (sourceWidth <= 0 || sourceHeight <= 0)
        {
            return new PixelSize(target, target);
        }

        // Keep icons crisp: never upscale above native resolution.
        var scale = Math.Min(1d, Math.Min((double)target / sourceWidth, (double)target / sourceHeight));
        var width = Math.Max(1, (int)Math.Round(sourceWidth * scale));
        var height = Math.Max(1, (int)Math.Round(sourceHeight * scale));
        return new PixelSize(width, height);
    }
}
