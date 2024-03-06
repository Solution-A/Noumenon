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

public class ManagerWindow : Window, IDisposable
{
    private Noumenon plugin;
    private string selectedDesignName = "No Design Selected";

    public ManagerWindow(Noumenon plugin) : base(
        "Noumenon Design Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(600, 430),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public override void Draw()
    {
        UI.tabBarHeader(plugin);
        //ImGui.Text($"The random config bool is {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(0, 0));
        if (ImGui.BeginTable("designFrame", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.NoPadInnerX 
            | ImGuiTableFlags.NoPadOuterX))
        {
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.3f);
            ImGui.TableSetupColumn("Column 2", ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoHide);

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);

            if (ImGui.BeginChild("designList", new Vector2(0, -ImGui.GetFrameHeightWithSpacing()), true, ImGuiWindowFlags.None))
            {
                UI.designList(selectedDesignName => this.selectedDesignName = selectedDesignName);
            }
            ImGui.EndChild();
            ImGui.TableSetColumnIndex(1);
            ImGuiEx.TextCentered(selectedDesignName);

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.PushItemWidth(-1);
            ImGui.Button("+");
            ImGui.PopItemWidth();
        }
        ImGui.EndTable();
        ImGui.PopStyleVar();
    }

    public void Dispose()
    {
    }
}
