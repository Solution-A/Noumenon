using Dalamud.Plugin;
using ImGuizmoNET;
using Penumbra.Api.Enums;
using Penumbra.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.IPC
{
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
        private FuncSubscriber<IList<(string, string)>> getMods;
        private FuncSubscriber<ApiCollectionType, string> currentCollection;
        private FuncSubscriber<int, (bool, bool, string)> objectCollection;
        private FuncSubscriber<string, string, string, bool, CurrentSettings> getCurrentSettings;
        public bool Available { get; private set; }
        public IReadOnlyList<(Mod Mod, ModSettings Settings)> GetMods()
        {
            if (!Available)
                return Array.Empty<(Mod Mod, ModSettings Settings)>();

            try
            {
                var allMods = getMods.Invoke();
                var collection = currentCollection.Invoke(ApiCollectionType.Current);
                return allMods
                    .Select(m => (m.Item1, m.Item2, getCurrentSettings.Invoke(collection, m.Item1, m.Item2, true)))
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
                ECommons.Logging.InternalLog.Error($"Error fetching mods from Penumbra:\n{ex}");
                return Array.Empty<(Mod Mod, ModSettings Settings)>();
            }
        }
        public string GetCurrentPlayerCollection()
        {
            if (!Available)
                return string.Empty;

            var (valid, _, name) = objectCollection.Invoke(0);
            return valid ? name : string.Empty;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
