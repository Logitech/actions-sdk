namespace Loupedeck.DemoPlugin
{
    using System;

    public class DemoApplication : ClientApplication
    {
        public DemoApplication()
        {

        }

        protected override String GetProcessName() => "DemoApplication";

        protected override String GetBundleName() => "";
    }
}