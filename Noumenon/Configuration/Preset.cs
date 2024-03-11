using Glamourer.Interop.Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.Configuration
{
    [Serializable]
    public class Preset
    {
        [NonSerialized] internal string guid = Guid.NewGuid().ToString();
        public string name = "";
        public Dictionary<Mod, ModSettings> modDictionary = new Dictionary<Mod, ModSettings>();
        public DesignListEntry design;

        public Preset(string name) 
        {
            this.name = name;
        }

        public void addMod(Mod mod, ModSettings modSettings)
        {
            modDictionary.Add(mod, modSettings);
        }

        public void removeMod(Mod mod) 
        {
            modDictionary.Remove(mod);
        }


    }
}
