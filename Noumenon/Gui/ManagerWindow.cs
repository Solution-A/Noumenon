using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons;
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
using Noumenon.Utils;
using System.Collections.Generic;

namespace Noumenon.Windows;

public class ManagerWindow : Window, IDisposable
{
    private Noumenon plugin;
    private string selectedDesignName = " Preset1_Preset1";
    string selectedSelectable = "#Preset1";
    bool presetSelected = false;
    bool presetEnabled = true;
    bool modEnabled = true;
    string presetNameInput = "";
    int currentDesignComboItem = 0;
    int currentModComboItem = 0;
    int modPrio = 0;
    GlamourerManager glamourerManager = new GlamourerManager();
    PenumbraService penumbraService = new PenumbraService();

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
            ImGui.TableSetupColumn("##columnFrame1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.2f);
            ImGui.TableSetupColumn("##columnFrame2", ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoHide);

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
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Clone);
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.FolderPlus);
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Trash);
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
            ImGui.TableSetupColumn("##columnPreset1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.05f);
            ImGui.TableSetupColumn("##columnPreset2", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.30f);
            ImGui.TableSetupColumn("##columnPreset3", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.65f);

            ImGui.TableHeadersRow();
            ImGui.TableSetColumnIndex(1);
            ImGui.Text("Name");
            ImGui.TableSetColumnIndex(2);
            ImGui.Text("Design");

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ElementUtils.alignInCol(ElementUtils.Alignment.Middle);
            ImGui.Checkbox("##checkPresetEnabled", ref presetEnabled);

            ImGui.TableSetColumnIndex(1);
            ImGuiEx.SetNextItemFullWidth();
            ImGui.InputTextWithHint("##name", "Preset name", ref presetNameInput, 100);

            ImGui.TableSetColumnIndex(2);
            DesignListEntry[] designListGlamourer = glamourerManager.GetDesigns();
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
            ImGui.Button("Try Applying All Associated Mods to Anima##softApplyMods");
            if (ImGui.BeginTable("preseFrame", 6, ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("##columnModAssociations1", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.1f);
                ImGui.TableSetupColumn("##columnModAssociations2", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.20f);
                ImGui.TableSetupColumn("##columnModAssociations3", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.17f);
                ImGui.TableSetupColumn("##columnModAssociations4", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.06f);
                ImGui.TableSetupColumn("##columnModAssociations5", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.08f);
                ImGui.TableSetupColumn("##columnModAssociations5", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide, ImGui.GetWindowWidth() * 0.12f);

                ImGui.TableHeadersRow();
                ImGui.TableSetColumnIndex(1);
                ImGui.Text("Mod Name");
                ImGui.TableSetColumnIndex(2);
                ImGui.Text("Directory Name");
                ImGui.TableSetColumnIndex(3);
                ImGuiEx.TextCentered("State");
                ImGui.TableSetColumnIndex(4);
                ImGuiEx.TextCentered("Priority");

                ImGui.TableNextRow(0);
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

                ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
                ImGui.TableSetColumnIndex(0);
                ElementUtils.alignInCol(ElementUtils.Alignment.Right);
                ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Plus);
                ImGui.TableSetColumnIndex(1);
                ImGuiEx.SetNextItemFullWidth();
                IReadOnlyList<(Mod Mod, ModSettings Settings)> modList = penumbraService.GetMods();
                ImGui.Combo("##penumbraModCombo", ref currentModComboItem, GuiLogic.modsToCombo(modList), modList.Count);

            }
            ImGui.EndTable();
        }
        else
        {
        }
    }

    private void glamourerDesignsToSelectables()
    {
        GuiLogic.designNamesToSelectables(selectedDesignName => this.selectedDesignName = selectedDesignName, ref presetSelected, ref selectedSelectable);
    }

    public void Dispose()
    {
    }
}
