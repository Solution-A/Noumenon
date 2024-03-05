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

public class MainWindow : Window
{
    private Noumenon plugin;
    public MainWindow(Noumenon plugin) : base(
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

        if (ImGui.Button("Show Settings"))
        {
            this.plugin.DrawConfigUI();
        }
        if (ImGui.BeginTable("designFrame", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable))
        {
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            if (ImGui.BeginTable("designList", 1, ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY))
            {
                for (int i = 0; i < 20; i++)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Selectable("Design" + (i + 1));
                }
            }
            ImGui.EndTable();
            ImGui.TableSetColumnIndex(1);
            ImGuiEx.LineCentered("DesignName", () =>
            {
                ImGuiEx.Text("Design Name");
            });
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Botones");
        }
        ImGui.EndTable();
    }

    public void Dispose()
    {

    }
}
