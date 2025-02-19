using System;

namespace Loupedeck.DemoPlugin
{
    public class DemoApplication : ClientApplication
    {
        public DemoApplication()
        {
        }

        // This method can be used to link the plugin to a Windows application.
        protected override string GetProcessName() => "DemoApplication";

        // This method can be used to link the plugin to a macOS application.
        protected override string GetBundleName() => "";
    }
}
