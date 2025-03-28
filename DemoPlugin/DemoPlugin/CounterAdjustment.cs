using System;

namespace Loupedeck.DemoPlugin
{
    // This class implements an example adjustment that counts the rotation ticks of a dial.
    public class CounterAdjustment : PluginDynamicAdjustment
    {
        // This variable holds the current value of the counter.
        private int counter = 0;

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Counter", description: "Counts rotation ticks", groupName: "Adjustments", hasReset: true)
        {
        }

        // This method is called when the dial associated to the plugin is rotated.
        protected override void ApplyAdjustment(string actionParameter, int diff)
        {
            counter += diff; // Increase or decrease the counter by the number of ticks.
            base.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
            PluginLog.Info($"Counter value was changed by {diff} ticks");
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(string actionParameter)
        {
            this.counter = 0; // Reset the counter.
            base.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
            PluginLog.Info("Counter was reset");
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override string GetAdjustmentValue(string actionParameter) => counter.ToString();
    }
}
