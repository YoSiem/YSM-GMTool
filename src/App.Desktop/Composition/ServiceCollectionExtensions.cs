using System.Reflection;
using App.Core.Abstractions;
using App.Core.Services;
using App.Data.Infrastructure;
using App.Data.Repositories;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGmTool(this IServiceCollection s, string appDir)
    {
        var queryFile = Path.Combine(AppContext.BaseDirectory, "Config", "queries.json");
        var settingsFile = Path.Combine(appDir, "settings.json");

        // Core services
        s.AddSingleton<IQueryStore>(_ => new FileQueryStore(queryFile));
        s.AddSingleton<IAppSettingsService>(_ => new JsonAppSettingsService(settingsFile));
        s.AddSingleton<INameNormalizer, SearchNameNormalizer>();
        s.AddSingleton<IConnectionStringBuilderService, DefaultConnectionStringBuilderService>();
        s.AddSingleton<ILocalCacheService>(_ => new LocalCacheService(appDir));
        s.AddSingleton<DbConnectionFactory>();
        s.AddSingleton<IGameDataRepository, GameDataRepository>();

        // Desktop infra
        s.AddSingleton<IClipboardService, AvaloniaClipboardService>();
        s.AddSingleton<IDialogService, DialogService>();
        s.AddSingleton<IAppSettingsHolder, AppSettingsHolder>();
        s.AddSingleton<IPlayerContext, PlayerContext>();
        s.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        s.AddSingleton<ConnectionStringResolver>();
        s.AddSingleton<IInventoryWindowService, InventoryWindowService>();
        s.AddSingleton<IItemEffectPickerService, ItemEffectPickerService>();

        // Shell
        s.AddSingleton<Shell.MainWindow>();
        s.AddSingleton<Shell.ShellViewModel>();
        s.AddSingleton<Shell.TopBarViewModel>();

        // Settings + About
        s.AddTransient<Features.Settings.SettingsViewModel>();
        s.AddTransient<Features.Settings.SettingsWindow>();
        s.AddTransient<Features.About.AboutWindow>();

        s.AddTabModules(typeof(ServiceCollectionExtensions).Assembly);
        return s;
    }

    /// <summary>Reflection-scan for ITabModule implementations and register each + IEnumerable&lt;ITabModule&gt;.</summary>
    public static IServiceCollection AddTabModules(this IServiceCollection s, Assembly asm)
    {
        foreach (var t in asm.GetTypes()
                     .Where(t => !t.IsAbstract && typeof(ITabModule).IsAssignableFrom(t)))
        {
            s.AddSingleton(t);
            s.AddSingleton(typeof(ITabModule), sp => (ITabModule)sp.GetRequiredService(t));
        }

        return s;
    }
}
