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
using Glamourer.Interop.Penumbra;
using OtterGui;
using Noumenon.Configuration;
using Dalamud.Interface.Utility.Raii;

namespace Noumenon.Windows;

public class ManagerWindow : Window, IDisposable
{
    private Noumenon plugin;
    DalamudPluginInterface pluginInterface;
    string selectedSelectable = "#Preset1";
    bool presetSelected = false;
    bool presetEnabled = true;
    bool modEnabled = true;
    string newName = "";
    string presetNameInput = "";
    List<Preset> presets = new List<Preset>();
    int currentDesignComboItem = 0;
    int currentModComboItem = 0;
    int modPrio = 0;
    GlamourerManager glamourerManager = new GlamourerManager();

    public ManagerWindow(Noumenon plugin, DalamudPluginInterface pi) : base(
        "Noumenon Design Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(650, 430),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        pluginInterface = pi;
        this.plugin = plugin;
    }

    public override void Draw()
    {
        ManagerWindowLogic.tabBarHeader(plugin);
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
            var itemSelectorPlus = new ItemSelector<Preset>(presets, ItemSelector<Preset>.Flags.Add);
            itemSelectorPlus.Draw(22);
            ImGui.SameLine();
            var itemSelectorClone = new ItemSelector<Preset>(presets, ItemSelector<Preset>.Flags.Duplicate);
            itemSelectorClone.Draw(22);
            ImGui.SameLine();
            ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.FolderPlus);
            ImGui.SameLine();
            var itemSelectorDelete = new ItemSelector<Preset>(presets, ItemSelector<Preset>.Flags.Delete);
            itemSelectorDelete.Draw(22);
            ImGui.PopItemWidth();
        }
        ImGui.EndTable();
        ImGui.PopStyleVar();
    }

    private void presetMenu() {
        ImGui.TableSetColumnIndex(1);
        ImGuiEx.TextCentered("uwu");
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
            ImGui.Combo("##glamourerDesignsCombo", ref currentDesignComboItem, ManagerWindowLogic.designListToNameList(designListGlamourer), designListGlamourer.Length);
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
            if (ImGui.BeginTable("##presetFrame", 6, ImGuiTableFlags.Borders))
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

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ElementUtils.alignInCol(ElementUtils.Alignment.Right);
                if (ImGuiEx.SmallIconButton(Dalamud.Interface.FontAwesomeIcon.Plus))
                {
                    //addNewMod();
                }
                ImGui.TableSetColumnIndex(1);
                ImGuiEx.SetNextItemFullWidth();
                PenumbraService penumbraService = new PenumbraService(pluginInterface);
                IReadOnlyList<(Mod Mod, ModSettings Settings)> modList = penumbraService.GetMods();
                ImGui.Combo("##penumbraModCombo", ref currentModComboItem, ManagerWindowLogic.modsToCombo(modList), modList.Count);
                ImGui.TableSetColumnIndex(2);
                ImGui.Text("Directory1");
                ImGui.TableSetColumnIndex(3);
                ElementUtils.alignInCol(ElementUtils.Alignment.Middle);
                ImGui.Checkbox("##checkPresetEnabled", ref modEnabled);
                ImGui.TableSetColumnIndex(4);
                ImGuiEx.SetNextItemFullWidth();
                ImGui.DragInt("", ref modPrio, 0.1f, 0, 99);

                ManagerWindowLogic.addMod(true, 6);

            }
            ImGui.EndTable();
        }
        else
        {
        }
    }

    private void glamourerDesignsToSelectables()
    {
        ManagerWindowLogic.designNamesToSelectables(selectedDesignName => this.newName = selectedDesignName, ref presetSelected, ref selectedSelectable);
    }

    public void Dispose()
    {
    }
}
