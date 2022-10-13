namespace Loupedeck.DemoPlugin
{
    using System;

    public class ToggleMuteCommand : PluginDynamicCommand
    {
        public ToggleMuteCommand()
            : base(displayName: "Toggle Mute", description: "Toggles audio mute state", groupName: "Audio")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.VolumeMute);
        }
    }
}
