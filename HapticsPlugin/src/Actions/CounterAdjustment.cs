namespace Loupedeck.HapticsPlugin
{
    using System;

    // This class implements an example adjustment that counts the rotation ticks of a dial and
    // demonstrates two more haptic waveform families: Collision (a subtle tick on every detent,
    // since it fires often) and Alert (a stronger completion cue on reset, since it fires rarely).

    public class CounterAdjustment : PluginDynamicAdjustment
    {
        private const String DialTickEvent = "dialTick";
        private const String CounterResetEvent = "counterReset";

        // This variable holds the current value of the counter.
        private Int32 _counter = 0;

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Tick Counter", description: "Counts rotation ticks", groupName: "Adjustments", hasReset: true)
        {
        }

        // This method is called when the plugin loads the adjustment. Register the haptic events here so
        // their names are known to the plugin before they are ever raised.
        protected override Boolean OnLoad()
        {
            this.Plugin.PluginEvents.AddEvent(DialTickEvent, "Dial Tick", "Fired every time the Tick Counter dial is rotated by one detent");
            this.Plugin.PluginEvents.AddEvent(CounterResetEvent, "Counter Reset", "Fired when the Tick Counter is reset to zero");
            return true;
        }

        // This method is called when the adjustment is executed.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            this._counter += diff; // Increase or decrease the counter by the number of ticks.
            this.AdjustmentValueChanged(); // Notify the plugin service that the adjustment value has changed.
            this.Plugin.PluginEvents.RaiseEvent(DialTickEvent);
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            this._counter = 0; // Reset the counter.
            this.AdjustmentValueChanged(); // Notify the plugin service that the adjustment value has changed.
            this.Plugin.PluginEvents.RaiseEvent(CounterResetEvent);
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) => this._counter.ToString();
    }
}
