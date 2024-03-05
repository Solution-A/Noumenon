using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System;
using System.Numerics;
using System.Reflection;

namespace Noumenon.Windows;

public class ManagerWindow : Window
{
    private Noumenon plugin;
    public ManagerWindow(Noumenon plugin) : base(
        "Noumenon Design Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(600, 440),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public override void Draw()
    {
        //ImGui.Text($"The random config bool is {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
        
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

    public void Dispose()
    {

    }
}
