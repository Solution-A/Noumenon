using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;

namespace Noumenon.Windows;

public class MainWindow : Window
{
    private Plugin plugin;

    public MainWindow(Plugin plugin) : base(
        "Noumenon Design Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(600, 600),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            this.plugin.DrawConfigUI();
        }
        ImGui.Text("Hola");

        ImGui.Indent(55);
        ImGui.Unindent(55);
    }

    public void Dispose(){ }
}
