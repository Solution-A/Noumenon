using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ImGuiNET;
using Noumenon.IPC;
using System;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using static FFXIVClientStructs.FFXIV.Client.Game.UI.ContentsFinder;

namespace Noumenon.Windows;

public class ManagerWindow : Window, IDisposable
{
    private Noumenon plugin;
    private string selectedDesignName = "No Design Selected";
    bool presetSelected = false;
    bool presetEnabled = true;
    bool modEnabled = true;
    string presetNameInput = "";
    int currentDesignComboItem = 0;
    int currentModComboItem = 0;
    string modPrio = "0";
    DesignListEntry[] designListGlamourer = GlamourerManager.GetDesigns();

    public ManagerWindow(Noumenon plugin) : base(
        "Noumenon Design Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(650, 430),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public override void Draw()
    {
        GuiLogic.tabBarHeader(plugin);
        //ImGui.Text($"The random config bool is {this.plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(0, 0));
        if (ImGui.BeginTable("designFrame", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.NoPadInnerX 
            | ImGuiTableFlags.NoPadOuterX))
        {
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.22f);
            ImGui.TableSetupColumn("Column 2", ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoHide);

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            if (ImGui.BeginChild("designListFrame", new Vector2(0, -ImGui.GetFrameHeightWithSpacing()), true, ImGuiWindowFlags.None))
            {
                glamourerDesignsToSelectables();
            }
            ImGui.EndChild();

            presetMenu();
            modAssociationsTab();

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.PushItemWidth(-1);
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Plus);
            ImGui.PopItemWidth();
        }
        ImGui.EndTable();
        ImGui.PopStyleVar();
    }

    private void presetMenu() {
        ImGui.TableSetColumnIndex(1);
        ImGuiEx.TextCentered(selectedDesignName);
        ImGui.Separator();
        if (ImGui.BeginTable("preseFrame", 3, ImGuiTableFlags.Borders))
        {
            ImGui.PushStyleColor(ImGuiCol.TableRowBg, ImGuiColors.DalamudGrey2);
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide);
            ImGui.PopStyleColor();

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(1);
            ImGui.Text("Name");
            ImGui.TableSetColumnIndex(2);
            ImGui.Text("Design");

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.Checkbox("##checkPresetEnabled", ref presetEnabled);

            ImGui.TableSetColumnIndex(1);
            ImGuiEx.SetNextItemFullWidth();
            ImGui.InputTextWithHint("##name", "Preset name", ref presetNameInput, 100);

            ImGui.TableSetColumnIndex(2);
            ImGuiEx.SetNextItemFullWidth();
            ImGui.Combo("##glamourerDesignsCombo", ref currentDesignComboItem, GuiLogic.designListToNameList(designListGlamourer), designListGlamourer.Length);
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Clone);
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Trash);

        }
        ImGui.EndTable();
    }

    private void modAssociationsTab()
    {
        if (ImGui.CollapsingHeader("Mod Associations##global", ImGuiTreeNodeFlags.DefaultOpen))
        {
            //ImGuiEx.SetNextItemFullWidth();
            //ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, 0f);
            ImGui.Button("Try Applying All Associated Mods to Anima##softApplyMods");
            //ImGui.PopStyleVar();
            if (ImGui.BeginTable("preseFrame", 6, ImGuiTableFlags.Borders))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(1);
                ImGui.Text("Mod Name");
                ImGui.TableSetColumnIndex(2);
                ImGui.Text("Directory Name");
                ImGui.TableSetColumnIndex(3);
                ImGui.Text("State");
                ImGui.TableSetColumnIndex(4);
                ImGui.Text("Priority");

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Trash);
                ImGui.SameLine();
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Sync);
                ImGui.TableSetColumnIndex(1);
                ImGui.Text("Mod1");
                ImGui.TableSetColumnIndex(2);
                ImGui.Text("Directory1");
                ImGui.TableSetColumnIndex(3);
                ImGui.Checkbox("##checkPresetEnabled", ref modEnabled);
                ImGui.TableSetColumnIndex(4);
                ImGuiEx.SetNextItemFullWidth();
                ImGui.InputTextWithHint("##prioNumber", "0", ref modPrio, 100);
                ImGui.TableSetColumnIndex(5);
                ImGui.Button("Try Applying");


                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Plus);
                ImGui.TableSetColumnIndex(1);
                ImGuiEx.SetNextItemFullWidth();
                ImGui.Combo("##penumbraModCombo", ref currentModComboItem, GuiLogic.designListToNameList(designListGlamourer), designListGlamourer.Length);


            }
            ImGui.EndTable();
        }
        else
        {
        }
    }

    private void glamourerDesignsToSelectables()
    {
        GuiLogic.designNamesToSelectables(selectedDesignName => this.selectedDesignName = selectedDesignName, ref presetSelected);
    }

    public void Dispose()
    {
    }
}
