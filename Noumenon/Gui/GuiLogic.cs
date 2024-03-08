using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ImGuiNET;
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
    public unsafe static class GuiLogic
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
        public static void designNamesToSelectables(Action<string> onSelectDesign, ref bool presetSelected)
        {
            for (int i = 0; i < 20; i++)
            {
                string selectableName = "Preset" + (i + 1) + "_Preset" + (i + 1);
                if (ImGui.Selectable(selectableName, ref presetSelected))
                {
                    onSelectDesign?.Invoke(selectableName);
                }
            }
        }

        public static string[] designListToNameList(DesignListEntry[] designList)
        {
            string[] designNamesArray = new string[designList.Length];
            Array.Sort(designList);
            for (int i = 0; i < designList.Length; i++)
            {
                string designName = designList[i].Name;
                designNamesArray[i] = designName;
            }
            return designNamesArray;
        }
    }
}
