using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.RandomOptions;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.RandomOption;

/// <summary>
/// Builds a <c>set_item_random_option(...)</c> command for Stat / Socket / Item effects. The stat
/// mode drives the option bitmask from a checkbox grid; the item mode fills the options from a
/// DB-backed picker. The live <see cref="Command"/> preview mirrors what Copy puts on the clipboard.
/// </summary>
public sealed class RandomOptionTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Random Option";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;
    private readonly IClipboardService _clipboard;
    private readonly IItemEffectPickerService _picker;

    private long _mask;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-dice";

    public override int Order => 35;

    public RandomOptionTabViewModel(
        ICommandDispatcher cmd,
        IPlayerContext player,
        IDialogService dlg,
        IClipboardService clipboard,
        IItemEffectPickerService picker)
    {
        _cmd = cmd;
        _player = player;
        _dlg = dlg;
        _clipboard = clipboard;
        _picker = picker;

        Equipparts = RandomOptionCatalog.Equipparts;
        _selectedEquippart = Equipparts[0];

        CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
        ImportOptions = ReactiveCommand.CreateFromTask(ImportAsync);
        PickEffect = ReactiveCommand.CreateFromTask(PickEffectAsync);

        CopyCommand.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: copy failed."));
        ImportOptions.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: import failed."));
        PickEffect.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: pick failed."));

        BuildFlags();
        Rebuild();
    }

    // --- Static lists for the view. ---

    public IReadOnlyList<RandomOptionEquippart> Equipparts { get; }

    public int[] Lines { get; } = Enumerable.Range(1, 10).ToArray();

    public int[] SocketTypes { get; } = [1, 2];

    public int[] SocketAmounts { get; } = [1, 2];

    public ObservableCollection<RandomOptionFlagViewModel> Flags { get; } = [];

    public ReactiveCommand<Unit, Unit> CopyCommand { get; }

    public ReactiveCommand<Unit, Unit> ImportOptions { get; }

    public ReactiveCommand<Unit, Unit> PickEffect { get; }

    // --- Mode. ---

    private RandomOptionKind _kind = RandomOptionKind.Stat;

    public RandomOptionKind Kind
    {
        get => _kind;
        set
        {
            if (_kind == value)
            {
                return;
            }

            _kind = value;
            this.RaisePropertyChanged(nameof(IsStatMode));
            this.RaisePropertyChanged(nameof(IsSocketMode));
            this.RaisePropertyChanged(nameof(IsItemMode));
            Rebuild();
        }
    }

    public bool IsStatMode { get => Kind == RandomOptionKind.Stat; set { if (value) { Kind = RandomOptionKind.Stat; } } }

    public bool IsSocketMode { get => Kind == RandomOptionKind.Socket; set { if (value) { Kind = RandomOptionKind.Socket; } } }

    public bool IsItemMode { get => Kind == RandomOptionKind.Item; set { if (value) { Kind = RandomOptionKind.Item; } } }

    // --- Universal inputs. ---

    private RandomOptionEquippart _selectedEquippart;

    public RandomOptionEquippart SelectedEquippart
    {
        get => _selectedEquippart;
        set { this.RaiseAndSetIfChanged(ref _selectedEquippart, value); Rebuild(); }
    }

    private int _selectedLine = 1;

    public int SelectedLine
    {
        get => _selectedLine;
        set { this.RaiseAndSetIfChanged(ref _selectedLine, value); Rebuild(); }
    }

    private bool _applyToOther;

    public bool ApplyToOther
    {
        get => _applyToOther;
        set { this.RaiseAndSetIfChanged(ref _applyToOther, value); Rebuild(); }
    }

    // --- Stat inputs. ---

    private bool _isPart2;

    public bool IsPart2
    {
        get => _isPart2;
        set { this.RaiseAndSetIfChanged(ref _isPart2, value); BuildFlags(); RecomputeMask(); Rebuild(); }
    }

    private bool _isPercentage;

    public bool IsPercentage
    {
        get => _isPercentage;
        set { this.RaiseAndSetIfChanged(ref _isPercentage, value); Rebuild(); }
    }

    private double _statValue;

    public double StatValue
    {
        get => _statValue;
        set { this.RaiseAndSetIfChanged(ref _statValue, value); Rebuild(); }
    }

    // --- Socket inputs. ---

    private int _socketType = 1;

    public int SocketType
    {
        get => _socketType;
        set { this.RaiseAndSetIfChanged(ref _socketType, value); Rebuild(); }
    }

    private int _socketAmount = 1;

    public int SocketAmount
    {
        get => _socketAmount;
        set { this.RaiseAndSetIfChanged(ref _socketAmount, value); Rebuild(); }
    }

    // --- Item inputs. ---

    private int? _itemEffectId;

    public int? ItemEffectId
    {
        get => _itemEffectId;
        private set { this.RaiseAndSetIfChanged(ref _itemEffectId, value); Rebuild(); }
    }

    private string _effectText = string.Empty;

    public string EffectText
    {
        get => _effectText;
        private set => this.RaiseAndSetIfChanged(ref _effectText, value);
    }

    private double _itemValue = 1;

    public double ItemValue
    {
        get => _itemValue;
        set { this.RaiseAndSetIfChanged(ref _itemValue, value); Rebuild(); }
    }

    // --- Derived (read-only display). ---

    private int _type;

    public int Type
    {
        get => _type;
        private set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    private long _optionsDisplay;

    public long OptionsDisplay
    {
        get => _optionsDisplay;
        private set => this.RaiseAndSetIfChanged(ref _optionsDisplay, value);
    }

    private string _command = string.Empty;

    public string Command
    {
        get => _command;
        private set => this.RaiseAndSetIfChanged(ref _command, value);
    }

    // --- Logic. ---

    private void BuildFlags()
    {
        Flags.Clear();
        var catalog = IsPart2 ? RandomOptionCatalog.Part2 : RandomOptionCatalog.Part1;
        foreach (var flag in catalog)
        {
            Flags.Add(new RandomOptionFlagViewModel(flag.Bit, flag.Label, OnFlagToggled));
        }
    }

    private void OnFlagToggled()
    {
        RecomputeMask();
        Rebuild();
    }

    private void RecomputeMask()
        => _mask = RandomOptionCalculator.ComputeMask(Flags.Where(f => f.IsChecked).Select(f => f.Bit));

    private int CurrentType() => Kind switch
    {
        RandomOptionKind.Stat => RandomOptionCalculator.DeriveStatType(IsPart2, IsPercentage),
        RandomOptionKind.Socket => 130,
        RandomOptionKind.Item => 133,
        _ => 0,
    };

    private long CurrentOptions() => Kind switch
    {
        RandomOptionKind.Stat => _mask,
        RandomOptionKind.Socket => SocketType,
        RandomOptionKind.Item => ItemEffectId ?? 0,
        _ => 0,
    };

    private double CurrentValue() => Kind switch
    {
        RandomOptionKind.Stat => StatValue,
        RandomOptionKind.Socket => SocketAmount,
        RandomOptionKind.Item => ItemValue,
        _ => 0,
    };

    private string BuildRaw()
    {
        var type = CurrentType();
        var options = CurrentOptions();
        var value = CurrentValue();
        var slot = SelectedEquippart.Index;

        return ApplyToOther && _player.TryResolveRequired(out var p)
            ? LuaCommands.SetItemRandomOptionPlayer(slot, p, SelectedLine, type, options, value)
            : LuaCommands.SetItemRandomOptionOwn(slot, SelectedLine, type, options, value);
    }

    private void Rebuild()
    {
        Type = CurrentType();
        OptionsDisplay = CurrentOptions();
        Command = _cmd.Format(BuildRaw());
    }

    private async Task CopyAsync()
    {
        if (ApplyToOther && !_player.TryResolveRequired(out _))
        {
            await _dlg.ShowWarningAsync(Title_, "Select player in the right sidebar for 'Other' target.");
            return;
        }

        if (Kind == RandomOptionKind.Item && (ItemEffectId is null || ItemEffectId <= 0))
        {
            await _dlg.ShowWarningAsync(Title_, "Pick an item effect first.");
            return;
        }

        await _cmd.DispatchAsync(BuildRaw());
    }

    private async Task ImportAsync()
    {
        var text = await _clipboard.GetTextAsync();
        if (string.IsNullOrWhiteSpace(text)
            || !long.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var mask)
            || mask < 0
            || mask > uint.MaxValue)
        {
            await _dlg.ShowWarningAsync(Title_, "Clipboard does not contain a valid options number.");
            return;
        }

        var catalog = IsPart2 ? RandomOptionCatalog.Part2 : RandomOptionCatalog.Part1;
        var bits = RandomOptionCalculator.BitsFromMask(mask, catalog).ToHashSet();
        foreach (var f in Flags)
        {
            f.SetCheckedSilently(bits.Contains(f.Bit));
        }

        RecomputeMask();
        Rebuild();
    }

    private async Task PickEffectAsync()
    {
        var picked = await _picker.PickAsync();
        if (picked is null)
        {
            return;
        }

        EffectText = picked.EffectText;
        ItemEffectId = picked.EffectId; // setter triggers Rebuild
    }
}
