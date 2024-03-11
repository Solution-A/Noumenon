using Dalamud.Interface;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using Glamourer.Gui.Tabs.DesignTab;
using Glamourer.Interop.Penumbra;
using ImGuiNET;
using OtterGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Noumenon.Gui.ModLink
{
    public class ModLinkTab
    {
        /*private readonly PenumbraService _penumbra;
        private readonly DesignFileSystemSelector _selector;
        private readonly DesignManager _manager;
        private readonly ModCombo _modCombo;

        public ModLinkTab(PenumbraService penumbra, DesignFileSystemSelector selector, DesignManager manager)
        {
            _penumbra = penumbra;
            _selector = selector;
            _manager = manager;
            _modCombo = new ModCombo(penumbra, Glamourer.Log);
        }

        private void DrawNewModRow()
        {
            var currentName = _modCombo.CurrentSelection.Mod.Name;
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            var tt = currentName.IsNullOrEmpty()
                ? "Please select a mod first."
                : _selector.Selected!.AssociatedMods.ContainsKey(_modCombo.CurrentSelection.Mod)
                    ? "The design already contains an association with the selected mod."
                    : string.Empty;

            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), new Vector2(ImGui.GetFrameHeight()), tt, tt.Length > 0,
                    true))
                _manager.AddMod(_selector.Selected!, _modCombo.CurrentSelection.Mod, _modCombo.CurrentSelection.Settings);
            ImGui.TableNextColumn();
            _modCombo.Draw("##new", currentName.IsNullOrEmpty() ? "Select new Mod..." : currentName, string.Empty,
                ImGui.GetContentRegionAvail().X, ImGui.GetTextLineHeight());
        }
    }*/
    }
}
