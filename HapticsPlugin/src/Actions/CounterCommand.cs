namespace Loupedeck.HapticsPlugin
{
    using System;

    // This class implements an example command that counts button presses and
    // demonstrates two haptic waveform families: State Change (every press) and
    // Special (every 10th press, as a celebratory milestone).

    public class CounterCommand : PluginDynamicCommand
    {
        private const String ButtonPressEvent = "buttonPress";
        private const String MilestoneEvent = "milestoneReached";

        private Int32 _counter = 0;

        // Initializes the command class.
        public CounterCommand()
            : base(displayName: "Press Counter", description: "Counts button presses", groupName: "Commands")
        {
        }

        // This method is called when the plugin loads the command. Register the haptic events here so
        // their names are known to the plugin before they are ever raised.
        protected override Boolean OnLoad()
        {
            this.Plugin.PluginEvents.AddEvent(ButtonPressEvent, "Button Press", "Fired every time the Press Counter button is pressed");
            this.Plugin.PluginEvents.AddEvent(MilestoneEvent, "Milestone Reached", "Fired every 10th button press");
            return true;
        }

        // This method is called when the user executes the command.
        protected override void RunCommand(String actionParameter)
        {
            this._counter++;
            this.ActionImageChanged(); // Notify the plugin service that the command display name and/or image has changed.
            PluginLog.Info($"Counter value is {this._counter}"); // Write the current counter value to the log file.

            this.Plugin.PluginEvents.RaiseEvent(ButtonPressEvent);
            if (this._counter % 10 == 0)
            {
                this.Plugin.PluginEvents.RaiseEvent(MilestoneEvent);
            }
        }

        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"Press Counter{Environment.NewLine}{this._counter}";
    }
}
