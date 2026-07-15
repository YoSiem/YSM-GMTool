using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Characters;
using App.Core.Commands;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.Character;

/// <summary>
/// Builds a <c>sv('key',value[,'player'])</c> / <c>av('key',value[,'player'])</c> command from a
/// curated attribute dropdown (or a free-text custom key), a Set/Add toggle, and an Own/Other
/// target. Quick actions just configure the builder. The live <see cref="Command"/> preview
/// mirrors what Copy puts on the clipboard.
/// </summary>
public sealed class CharacterTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Character";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-user";

    public override int Order => 15;

    public CharacterTabViewModel(ICommandDispatcher cmd, IPlayerContext player, IDialogService dlg)
    {
        _cmd = cmd;
        _player = player;
        _dlg = dlg;

        Attributes = PlayerValueCatalog.Attributes;
        _selectedAttribute = Attributes[0];

        CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
        SetLevelPreset = ReactiveCommand.Create(() => ApplyPreset(CharacterOp.Set, "level"));
        AddExpPreset = ReactiveCommand.Create(() => ApplyPreset(CharacterOp.Add, "exp"));
        AddJpPreset = ReactiveCommand.Create(() => ApplyPreset(CharacterOp.Add, "jp"));
        AddGoldPreset = ReactiveCommand.Create(() => ApplyPreset(CharacterOp.Add, "gold"));

        CopyCommand.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Character: copy failed."));

        Rebuild();
    }

    // --- Static lists for the view. ---

    public IReadOnlyList<PlayerValueAttribute> Attributes { get; }

    public ReactiveCommand<Unit, Unit> CopyCommand { get; }

    public ReactiveCommand<Unit, Unit> SetLevelPreset { get; }

    public ReactiveCommand<Unit, Unit> AddExpPreset { get; }

    public ReactiveCommand<Unit, Unit> AddJpPreset { get; }

    public ReactiveCommand<Unit, Unit> AddGoldPreset { get; }

    // --- Operation (Set / Add). ---

    private CharacterOp _op = CharacterOp.Set;

    public CharacterOp Op
    {
        get => _op;
        set
        {
            if (_op == value)
            {
                return;
            }

            _op = value;
            this.RaisePropertyChanged(nameof(IsSet));
            this.RaisePropertyChanged(nameof(IsAdd));
            this.RaisePropertyChanged(nameof(ValueLabel));
            Rebuild();
        }
    }

    public bool IsSet { get => Op == CharacterOp.Set; set { if (value) { Op = CharacterOp.Set; } } }

    public bool IsAdd { get => Op == CharacterOp.Add; set { if (value) { Op = CharacterOp.Add; } } }

    public string ValueLabel => IsAdd ? "Amount" : "Value";

    // --- Attribute / custom key. ---

    private PlayerValueAttribute _selectedAttribute;

    public PlayerValueAttribute SelectedAttribute
    {
        get => _selectedAttribute;
        set { this.RaiseAndSetIfChanged(ref _selectedAttribute, value); Rebuild(); }
    }

    private bool _useCustomKey;

    public bool UseCustomKey
    {
        get => _useCustomKey;
        set { this.RaiseAndSetIfChanged(ref _useCustomKey, value); Rebuild(); }
    }

    private string _customKey = string.Empty;

    public string CustomKey
    {
        get => _customKey;
        set { this.RaiseAndSetIfChanged(ref _customKey, value); Rebuild(); }
    }

    // --- Value / target. ---

    private decimal _value;

    public decimal Value
    {
        get => _value;
        set { this.RaiseAndSetIfChanged(ref _value, value); Rebuild(); }
    }

    private bool _applyToOther;

    public bool ApplyToOther
    {
        get => _applyToOther;
        set { this.RaiseAndSetIfChanged(ref _applyToOther, value); Rebuild(); }
    }

    // --- Derived. ---

    private string _command = string.Empty;

    public string Command
    {
        get => _command;
        private set => this.RaiseAndSetIfChanged(ref _command, value);
    }

    // --- Logic. ---

    private string CurrentKey() => UseCustomKey ? (CustomKey ?? string.Empty).Trim() : SelectedAttribute.Key;

    private void ApplyPreset(CharacterOp op, string key)
    {
        UseCustomKey = false;
        var match = Attributes.FirstOrDefault(a => a.Key == key);
        if (!string.IsNullOrEmpty(match.Key))
        {
            SelectedAttribute = match;
        }

        Op = op;
    }

    private string BuildRaw()
    {
        var key = CurrentKey();
        // Round to the nearest integer so the emitted value matches the "0"-formatted display
        // (the spinner allows typing a fractional value; game values are whole numbers).
        var value = (long)Math.Round(Value, MidpointRounding.AwayFromZero);

        if (ApplyToOther && _player.TryResolveRequired(out var p))
        {
            return Op == CharacterOp.Add
                ? LuaCommands.AddValuePlayer(key, value, p)
                : LuaCommands.SetValuePlayer(key, value, p);
        }

        return Op == CharacterOp.Add
            ? LuaCommands.AddValueOwn(key, value)
            : LuaCommands.SetValueOwn(key, value);
    }

    private void Rebuild() => Command = _cmd.Format(BuildRaw());

    private async Task CopyAsync()
    {
        if (ApplyToOther && !_player.TryResolveRequired(out _))
        {
            await _dlg.ShowWarningAsync(Title_, "Select a player in the right sidebar for the 'Other' target.");
            return;
        }

        if (UseCustomKey && string.IsNullOrWhiteSpace(CustomKey))
        {
            await _dlg.ShowWarningAsync(Title_, "Enter a custom key, or untick 'Custom key'.");
            return;
        }

        await _cmd.DispatchAsync(BuildRaw());
    }
}
