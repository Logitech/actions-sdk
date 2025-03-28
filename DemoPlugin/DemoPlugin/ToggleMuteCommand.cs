using System;

namespace Loupedeck.DemoPlugin
{
    public class ToggleMuteCommand : PluginDynamicCommand
    {
        public ToggleMuteCommand()
            : base(
                  displayName: "Toggle Mute", 
                  description: "Mutes and unmutes system volume", 
                  groupName: "Audio")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            base.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.VolumeMute);
        }
    }
}
