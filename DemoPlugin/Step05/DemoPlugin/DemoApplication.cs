namespace Loupedeck.DemoPlugin
{
    using System;

    public class DemoApplication : ClientApplication
    {
        public DemoApplication()
        {
        }

        // This method can be used to link the plugin to a Windows application.
        protected override String GetProcessName() => "DemoApplication";

        // This method can be used to link the plugin to a macOS application.
        protected override String GetBundleName() => "";
    }
}