using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Utility;
using ECommons;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using Glamourer.Interop.Penumbra;
using ImGuiNET;
using Noumenon.IPC;
using Noumenon.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;


namespace Noumenon.Windows
{
    public unsafe static class ManagerWindowLogic
    {
        /*public static void DrawMain()
        {
            KoFiButton.DrawRight();
            ImGuiEx.EzTabBar("Tabs", true, [
                ("Manager", ManagerWindowTest.Draw, null, true),
                InternalLog.ImGuiTab(),
                //("Settings", configWindow.Draw, null, true)
            ]);
        }*/
        public static void tabBarHeader(Noumenon noumenon)
        {
            if (ImGui.BeginTabBar("mainTabBar", ImGuiTabBarFlags.NoTooltip |
                ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.NoTabListScrollingButtons))
            {
                if (ImGui.BeginTabItem("Manager"))
                {
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Settings"))
                {
                    noumenon.DrawConfigUI();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }
        public static void designNamesToSelectables(Action<string> onSelectDesign, ref bool presetSelected, ref string selectedSelectable)
        {
            for (int i = 0; i < 20; i++)
            {
                string selectableID= "#Preset" + (i+1);
                if (selectedSelectable.Equals(selectableID))
                {
                    presetSelected = true;
                }
                string selectableName = " Preset" + (i + 1) + "_Preset" + (i + 1);
                if (ImGui.Selectable(selectableName, ref presetSelected))
                {
                    selectedSelectable = selectableID;
                    onSelectDesign?.Invoke(selectableName);
                }
                presetSelected = false;
            }
        }

        public static string[] designListToNameList(DesignListEntry[] designListGlamourer)
        {
            string[] designNamesArray = new string[designListGlamourer.Length];
            Array.Sort(designListGlamourer);
            for (int i = 0; i < designListGlamourer.Length; i++)
            {
                string designName = designListGlamourer[i].Name;
                designNamesArray[i] = designName;
            }
            return designNamesArray;
        }

        public static string[] modsToCombo(IReadOnlyList<(Mod Mod, ModSettings Settings)> modList)
        {
            int modCount = modList.Count;
            string[] modNameList = new string[modCount];
            for (int i = 0; i < modCount; i++)
            {
                modNameList[i] = modList[i].Mod.Name;
            }
            return modNameList;
        }

        public static void addMod(bool modEnabled, int modPrio)
        {
            if (ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Plus))
            {
                ImGui.TableSetColumnIndex(0);
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Trash);
                ImGui.SameLine();
                ElementUtils.alignInCol(ElementUtils.Alignment.Right);
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Sync);
                ImGui.TableSetColumnIndex(1);
                ImGui.Text("Mod1");
                ImGui.TableSetColumnIndex(2);
                ImGui.Text("Directory1");
                ImGui.TableSetColumnIndex(3);
                ElementUtils.alignInCol(ElementUtils.Alignment.Middle);
                ImGui.Checkbox("##checkPresetEnabled", ref modEnabled);
                ImGui.TableSetColumnIndex(4);
                ImGuiEx.SetNextItemFullWidth();
                ImGui.DragInt("", ref modPrio, 0.1f, 0, 99);
                ImGui.TableSetColumnIndex(5);
                ElementUtils.setNextItemFullWidthCol();
                ImGui.Button("Try Applying");
                ImGui.TableNextRow();
                PluginLog.Debug($"Applying design");

            }
        }
    }
}
