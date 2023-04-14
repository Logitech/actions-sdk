﻿namespace Loupedeck.DemoPlugin
{
    using System;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class CounterAdjustment : PluginDynamicAdjustment
    {
        // This variable holds the current value of the counter.
        private Int32 _counter = 0;

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Counter", description: "Counts rotation ticks", groupName: "Adjustments", hasReset: true)
        {
        }

        // This method is called when the dial associated to the plugin is rotated.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            this._counter += diff; // Increase or decrease the counter by the number of ticks.
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
            PluginLog.Info($"Counter value was changed by {diff} ticks");
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            this._counter = 0; // Reset the counter.
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
            PluginLog.Info("Counter was reset");
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) => this._counter.ToString();
    }
}
