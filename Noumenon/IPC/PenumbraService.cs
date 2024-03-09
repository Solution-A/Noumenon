using Dalamud.Interface.Internal.Notifications;
using Dalamud.Plugin;
using Penumbra.Api;
using Penumbra.Api.Enums;
using Penumbra.Api.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System;
using ECommons.Logging;
using System.Linq;

namespace Glamourer.Interop.Penumbra;

using CurrentSettings = ValueTuple<PenumbraApiEc, (bool, int, IDictionary<string, IList<string>>, bool)?>;

public readonly record struct Mod(string Name, string DirectoryName) : IComparable<Mod>
{
    public int CompareTo(Mod other)
    {
        var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
        if (nameComparison != 0)
            return nameComparison;

        return string.Compare(DirectoryName, other.DirectoryName, StringComparison.Ordinal);
    }
}

public readonly record struct ModSettings(IDictionary<string, IList<string>> Settings, int Priority, bool Enabled)
{
    public ModSettings()
        : this(new Dictionary<string, IList<string>>(), 0, false)
    { }

    public static ModSettings Empty
        => new();
}

public unsafe class PenumbraService : IDisposable
{
    public const int RequiredPenumbraBreakingVersion = 4;
    public const int RequiredPenumbraFeatureVersion = 15;

    private readonly DalamudPluginInterface _pluginInterface;
    private readonly EventSubscriber<ChangedItemType, uint> _tooltipSubscriber;
    private readonly EventSubscriber<MouseButton, ChangedItemType, uint> _clickSubscriber;
    private readonly EventSubscriber<nint, string, nint, nint, nint> _creatingCharacterBase;
    private readonly EventSubscriber<nint, string, nint> _createdCharacterBase;
    private readonly EventSubscriber<ModSettingChange, string, string, bool> _modSettingChanged;
    private FuncSubscriber<IList<(string, string)>> _getMods;
    private FuncSubscriber<ApiCollectionType, string> _currentCollection;
    private FuncSubscriber<string, string, string, bool, CurrentSettings> _getCurrentSettings;

    private readonly EventSubscriber _initializedEvent;
    private readonly EventSubscriber _disposedEvent;

    public bool Available { get; private set; }

    public PenumbraService(DalamudPluginInterface pi)
    {
        _pluginInterface = pi;
        _initializedEvent = Ipc.Initialized.Subscriber(pi, Reattach);
        _disposedEvent = Ipc.Disposed.Subscriber(pi, Unattach);
        _tooltipSubscriber = Ipc.ChangedItemTooltip.Subscriber(pi);
        _clickSubscriber = Ipc.ChangedItemClick.Subscriber(pi);
        _createdCharacterBase = Ipc.CreatedCharacterBase.Subscriber(pi);
        _creatingCharacterBase = Ipc.CreatingCharacterBase.Subscriber(pi);
        _modSettingChanged = Ipc.ModSettingChanged.Subscriber(pi);
        Reattach();
    }

    public IReadOnlyList<(Mod Mod, ModSettings Settings)> GetMods()
    {
        if (!Available)
            return Array.Empty<(Mod Mod, ModSettings Settings)>();

        try
        {
            var allMods = _getMods.Invoke();
            var collection = _currentCollection.Invoke(ApiCollectionType.Current);
            return allMods
                .Select(m => (m.Item1, m.Item2, _getCurrentSettings.Invoke(collection, m.Item1, m.Item2, true)))
                .Where(t => t.Item3.Item1 is PenumbraApiEc.Success)
                .Select(t => (new Mod(t.Item2, t.Item1),
                    !t.Item3.Item2.HasValue
                        ? ModSettings.Empty
                        : new ModSettings(t.Item3.Item2!.Value.Item3, t.Item3.Item2!.Value.Item2, t.Item3.Item2!.Value.Item1)))
                .OrderByDescending(p => p.Item2.Enabled)
                .ThenBy(p => p.Item1.Name)
                .ThenBy(p => p.Item1.DirectoryName)
                .ThenByDescending(p => p.Item2.Priority)
                .ToList();
        }
        catch (Exception ex)
        {
            InternalLog.Error($"Error fetching mods from Penumbra:\n{ex}");
            return Array.Empty<(Mod Mod, ModSettings Settings)>();
        }
    }

    public void Reattach()
    {
        try
        {
            Unattach();

            var (breaking, feature) = Ipc.ApiVersions.Subscriber(_pluginInterface).Invoke();
            if (breaking != RequiredPenumbraBreakingVersion || feature < RequiredPenumbraFeatureVersion)
                throw new Exception(
                    $"Invalid Version {breaking}.{feature:D4}, required major Version {RequiredPenumbraBreakingVersion} with feature greater or equal to {RequiredPenumbraFeatureVersion}.");

            _tooltipSubscriber.Enable();
            _clickSubscriber.Enable();
            _creatingCharacterBase.Enable();
            _createdCharacterBase.Enable();
            _modSettingChanged.Enable();
            _getMods = Ipc.GetMods.Subscriber(_pluginInterface);
            _currentCollection = Ipc.GetCollectionForType.Subscriber(_pluginInterface);
            _getCurrentSettings = Ipc.GetCurrentModSettings.Subscriber(_pluginInterface);
            Available = true;
            InternalLog.Debug("Noumenon attached to Penumbra.");
        }
        catch (Exception e)
        {
            InternalLog.Debug($"Could not attach to Penumbra:\n{e}");
        }
    }

    /// <summary> Unattach from the currently running Penumbra IPC provider. </summary>
    public void Unattach()
    {
        _tooltipSubscriber.Disable();
        _clickSubscriber.Disable();
        _creatingCharacterBase.Disable();
        _createdCharacterBase.Disable();
        _modSettingChanged.Disable();
        if (Available)
        {
            Available = false;
            InternalLog.Debug("Noumenon detached from Penumbra.");
        }
    }

    public void Dispose()
    {
        Unattach();
        _tooltipSubscriber.Dispose();
        _clickSubscriber.Dispose();
        _creatingCharacterBase.Dispose();
        _createdCharacterBase.Dispose();
        _initializedEvent.Dispose();
        _disposedEvent.Dispose();
        _modSettingChanged.Dispose();
    }
}
