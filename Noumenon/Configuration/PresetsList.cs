using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.Configuration
{
    public class PresetsList
    {
        private List<Preset> presetsList = new List<Preset>();
        public IReadOnlyList<Preset> Presets => presetsList;

        public void AddPreset(Preset preset)
        {
            presetsList.Add(preset);
        }
        public void RemovePreset(Preset preset)
        {
            presetsList.Remove(preset);
        }
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(presetsList);
            File.WriteAllText(filePath, json);
        }
        public static PresetsList LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new PresetsList();

            var json = File.ReadAllText(filePath);
            var presets = JsonConvert.DeserializeObject<List<Preset>>(json);
            return new PresetsList { presetsList = presets };
        }
    }
}
