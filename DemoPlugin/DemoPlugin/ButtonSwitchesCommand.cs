using System;

namespace Loupedeck.DemoPlugin
{
    public class ButtonSwitchesCommand : PluginDynamicCommand
    {
        private const int NumberOfSwitches = 4;
        private readonly bool[] switches = new bool[NumberOfSwitches];

        public ButtonSwitchesCommand() : base()
        {
            for (int i = 0; i < NumberOfSwitches; i++)
            {
                // Parameter is the switch index
                var actionParameter = i.ToString();

                // Add parameter
                base.AddParameter(actionParameter, $"Switch {i}", "Switches");
            }
        }

        protected override void RunCommand(string actionParameter)
        {
            if (int.TryParse(actionParameter, out var i))
            {
                // Turn the switch
                this.switches[i] = !switches[i];

                // Inform Loupedeck that command display name and/or image has changed
                base.ActionImageChanged(actionParameter);
            }
        }

        protected override string GetCommandDisplayName(string actionParameter, PluginImageSize imageSize)
        {
            if (int.TryParse(actionParameter, out int i))
            {
                return $"Switch {i}: {switches[i]}";
            }
            else
            {
                return null;
            }
        }
    }
}
