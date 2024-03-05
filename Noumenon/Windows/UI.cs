using Dalamud.Interface.Colors;
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

namespace Noumenon.Gui
{
    public unsafe static class UI
    {
        public static void DrawMain()
        {
            KoFiButton.DrawRight();
            ImGuiEx.EzTabBar("Tabs", true, [
                ("Manager", ManagerWindowTest.Draw, null, true),
                InternalLog.ImGuiTab(),
                //("Settings", configWindow.Draw, null, true)
            ]);
        }
    }
}
