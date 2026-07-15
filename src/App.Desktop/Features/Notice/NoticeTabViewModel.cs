using System;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using Avalonia.Media;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.Notice;

/// <summary>
/// Builds a global <c>notice/notice_left/notice_right/announce("...")</c> command with optional
/// color (<c>&lt;#rrggbb&gt;</c>), size (<c>&lt;size:n&gt;</c>) and bold (<c>&lt;B&gt;</c>) tags,
/// multi-line text joined with <c>&lt;BR&gt;</c>. A live preview approximates the in-game render;
/// the <see cref="Command"/> preview mirrors what Copy puts on the clipboard.
/// </summary>
public sealed class NoticeTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Notice";
    private const string DefaultNoticeColor = "#FFEA00"; // client default for notice variants
    private const string DefaultAnnounceColor = "#00FF00"; // client default for announce

    private readonly ICommandDispatcher _cmd;
    private readonly IDialogService _dlg;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-bullhorn";

    public override int Order => 100;

    public NoticeTabViewModel(ICommandDispatcher cmd, IDialogService dlg)
    {
        _cmd = cmd;
        _dlg = dlg;

        CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
        PickColor = ReactiveCommand.Create<string>(hex => { ColorHex = hex; UseCustomColor = true; });

        CopyCommand.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Notice: copy failed."));

        Rebuild();
    }

    public ReactiveCommand<Unit, Unit> CopyCommand { get; }

    public ReactiveCommand<string, Unit> PickColor { get; }

    // --- Kind (alignment / function). ---

    private NoticeKind _kind = NoticeKind.Center;

    public NoticeKind Kind
    {
        get => _kind;
        set
        {
            if (_kind == value)
            {
                return;
            }

            _kind = value;
            this.RaisePropertyChanged(nameof(IsCenter));
            this.RaisePropertyChanged(nameof(IsLeft));
            this.RaisePropertyChanged(nameof(IsRight));
            this.RaisePropertyChanged(nameof(IsAnnounce));
            Rebuild();
        }
    }

    public bool IsCenter { get => Kind == NoticeKind.Center; set { if (value) { Kind = NoticeKind.Center; } } }

    public bool IsLeft { get => Kind == NoticeKind.Left; set { if (value) { Kind = NoticeKind.Left; } } }

    public bool IsRight { get => Kind == NoticeKind.Right; set { if (value) { Kind = NoticeKind.Right; } } }

    public bool IsAnnounce { get => Kind == NoticeKind.Announce; set { if (value) { Kind = NoticeKind.Announce; } } }

    // --- Text. ---

    private string _text = "Welcome!";

    public string Text
    {
        get => _text;
        set { this.RaiseAndSetIfChanged(ref _text, value); Rebuild(); }
    }

    // --- Color. ---

    private bool _useCustomColor;

    public bool UseCustomColor
    {
        get => _useCustomColor;
        set { this.RaiseAndSetIfChanged(ref _useCustomColor, value); Rebuild(); }
    }

    private string _colorHex = "#FF3B30";

    public string ColorHex
    {
        get => _colorHex;
        set { this.RaiseAndSetIfChanged(ref _colorHex, value); Rebuild(); }
    }

    // --- Size. ---

    private bool _useCustomSize;

    public bool UseCustomSize
    {
        get => _useCustomSize;
        set { this.RaiseAndSetIfChanged(ref _useCustomSize, value); Rebuild(); }
    }

    private int _fontSize = 20;

    public int FontSize
    {
        get => _fontSize;
        set { this.RaiseAndSetIfChanged(ref _fontSize, value); Rebuild(); }
    }

    // --- Bold. ---

    private bool _bold;

    public bool Bold
    {
        get => _bold;
        set { this.RaiseAndSetIfChanged(ref _bold, value); Rebuild(); }
    }

    // --- Derived: command. ---

    private string _command = string.Empty;

    public string Command
    {
        get => _command;
        private set => this.RaiseAndSetIfChanged(ref _command, value);
    }

    // --- Derived: preview. ---

    private string _previewText = string.Empty;

    public string PreviewText
    {
        get => _previewText;
        private set => this.RaiseAndSetIfChanged(ref _previewText, value);
    }

    private IBrush _previewBrush = Brushes.Yellow;

    public IBrush PreviewBrush
    {
        get => _previewBrush;
        private set => this.RaiseAndSetIfChanged(ref _previewBrush, value);
    }

    private double _previewFontSize = 17;

    public double PreviewFontSize
    {
        get => _previewFontSize;
        private set => this.RaiseAndSetIfChanged(ref _previewFontSize, value);
    }

    private FontWeight _previewFontWeight = FontWeight.SemiBold;

    public FontWeight PreviewFontWeight
    {
        get => _previewFontWeight;
        private set => this.RaiseAndSetIfChanged(ref _previewFontWeight, value);
    }

    private TextAlignment _previewAlignment = TextAlignment.Center;

    public TextAlignment PreviewAlignment
    {
        get => _previewAlignment;
        private set => this.RaiseAndSetIfChanged(ref _previewAlignment, value);
    }

    // --- Logic. ---

    private string DefaultColor() => Kind == NoticeKind.Announce ? DefaultAnnounceColor : DefaultNoticeColor;

    private string Wrap(string payload) => Kind switch
    {
        NoticeKind.Left => LuaCommands.NoticeLeft(payload),
        NoticeKind.Right => LuaCommands.NoticeRight(payload),
        NoticeKind.Announce => LuaCommands.Announce(payload),
        _ => LuaCommands.Notice(payload),
    };

    private void Rebuild()
    {
        var hex = UseCustomColor ? HexColor.Normalize(ColorHex) : null;
        int? size = UseCustomSize ? FontSize : null;

        var payload = NoticeTextBuilder.Build(Text, Bold, hex, size);
        Command = _cmd.Format(Wrap(payload));

        UpdatePreview(hex);
    }

    private void UpdatePreview(string? hex)
    {
        PreviewText = Text;

        // Use only the RGB bytes for the preview (the game tag may carry an alpha byte as
        // <#rrggbbaa>, but Avalonia's Color.Parse reads 8-digit hex as #aarrggbb — different order).
        var rgb6 = hex is { Length: >= 6 } ? hex[..6] : null;
        var colorString = rgb6 is not null ? "#" + rgb6 : DefaultColor();
        PreviewBrush = TryBrush(colorString) ?? TryBrush(DefaultColor()) ?? Brushes.Yellow;

        PreviewFontSize = UseCustomSize ? Math.Clamp(FontSize * 1.3, 10, 48) : 17;
        PreviewFontWeight = Bold ? FontWeight.Bold : FontWeight.SemiBold;
        PreviewAlignment = Kind switch
        {
            NoticeKind.Left => TextAlignment.Left,
            NoticeKind.Right => TextAlignment.Right,
            _ => TextAlignment.Center,
        };
    }

    private static IBrush? TryBrush(string colorString)
    {
        try
        {
            return new SolidColorBrush(Color.Parse(colorString));
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private async Task CopyAsync()
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            await _dlg.ShowWarningAsync(Title_, "Enter notice text first.");
            return;
        }

        var hex = UseCustomColor ? HexColor.Normalize(ColorHex) : null;
        if (UseCustomColor && hex is null)
        {
            await _dlg.ShowWarningAsync(Title_, "Color is not a valid hex value (use #RRGGBB or #RRGGBBAA), or untick 'Custom color'.");
            return;
        }

        int? size = UseCustomSize ? FontSize : null;
        await _cmd.DispatchAsync(Wrap(NoticeTextBuilder.Build(Text, Bold, hex, size)));
    }
}
