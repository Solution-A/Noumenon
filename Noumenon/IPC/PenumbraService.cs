using Dalamud.Plugin;
using ImGuizmoNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.IPC
{
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
        private readonly DalamudPluginInterface _pluginInterface;
        private FuncSubscriber<IList<(string, string)>> _getMods;
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
                Glamourer.Log.Error($"Error fetching mods from Penumbra:\n{ex}");
                return Array.Empty<(Mod Mod, ModSettings Settings)>();
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
