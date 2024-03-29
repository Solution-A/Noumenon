using System;
using Dalamud.Configuration;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Noumenon.Windows;

public class ConfigWindow : Window
{
    private Configuration.Configuration configuration;

    public ConfigWindow(Noumenon plugin) : base(
        "A Wonderful Configuration Window",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Size = new Vector2(232, 75);
        this.SizeCondition = ImGuiCond.Always;
        this.configuration = plugin.Configuration;
    }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        var configValue = this.configuration.SomePropertyToBeSavedAndWithADefault;
        if (ImGui.Checkbox("Random Config Bool", ref configValue))
        {
            this.configuration.SomePropertyToBeSavedAndWithADefault = configValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.configuration.Save();
        }
    }
    public void Dispose(){ }
}
