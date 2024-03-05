using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System.Numerics;

namespace Noumenon.Gui;

public unsafe static class ManagerWindowTest
{
    static Vector2 iconSize => new(24f.Scale());
    static string[] Filters = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""];
    static bool[] OnlySelected = new bool[20];
    static string CurrentDrag = "";
    public static void Draw()
    {
        if (ImGui.BeginTable("designFrame", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | 
            ImGuiTableFlags.NoPadInnerX | ImGuiTableFlags.NoPadOuterX))
        {
            ImGuiStyleVar designFramePadding = ImGuiStyleVar.CellPadding;
            ImGui.PushStyleVar(designFramePadding, 0);
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            if (ImGui.BeginTable("designList", 1, ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY))
            {
                for (int i = 0; i < 20; i++)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Selectable("Design" + (i + 1), true);
                }
            }
            ImGui.EndTable();

            ImGui.TableSetColumnIndex(1);
            ImGuiEx.TextCentered("Design Name");
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.Button("+"); ;
        }
        ImGui.EndTable();
    }
}
