namespace Loupedeck.DemoPlugin
{
    using System;

    class ToggleMuteCommand : PluginDynamicCommand
    {
        public ToggleMuteCommand() : base("Toggle Mute", "Toggles audio mute state", "Audio")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.VolumeMute);
        }
    }
}
