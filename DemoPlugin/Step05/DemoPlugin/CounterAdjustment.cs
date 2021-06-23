namespace Loupedeck.DemoPlugin
{
    using System;

    class CounterAdjustment : PluginDynamicAdjustment
    {
        private Int32 _counter = 0;

        public CounterAdjustment() : base("Counter", "Counts rotation ticks", "Tools", true)
        {
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            this._counter += ticks; // increase or decrease counter on the number of ticks
            this.ActionImageChanged(actionParameter);
        }

        protected override void RunCommand(String actionParameter)
        {
            this._counter = 0; // reset counter to 0
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            return this._counter.ToString();
        }
    }
}
