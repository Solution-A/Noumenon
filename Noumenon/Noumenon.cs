using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using ECommons.Configuration;
using ECommons.SimpleGui;
using Noumenon.Windows;
using System.IO;
using System.Reflection;

namespace Noumenon
{
    public sealed class Noumenon : IDalamudPlugin
    {
        public string Name => "Noumenon";
        private const string CommandName = "/noumenon";

        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration.Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("Noumenon");

        private ConfigWindow ConfigWindow { get; init; }
        private ManagerWindow ManagerWindow { get; init; }

        public Noumenon(
            DalamudPluginInterface pluginInterface,
            ICommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            ECommonsMain.Init(PluginInterface, this, ECommons.Module.DalamudReflector);

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration.Configuration ?? new Configuration.Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            //var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            //var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            ManagerWindow = new ManagerWindow(this, PluginInterface);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(ManagerWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            //EzConfigGui.Init(UI.DrawMain);
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ECommonsMain.Dispose();
            ConfigWindow.Dispose();
            ManagerWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            ManagerWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
