namespace Loupedeck.DemoPlugin
{
    using System;

    public class DemoPlugin : Plugin
    {
        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is a Universal plugin or an Application plugin.
        public override Boolean HasNoApplication => true;

        // Initializes a new instance of the plugin class.
        public DemoPlugin()
        {
            // Initialize the plugin log.
            PluginLog.Init(this.Log);

            // Initialize the plugin resources.
            PluginResources.Init(this.Assembly);
        }

        // This method is called when the plugin is loaded during the Loupedeck service start-up.
        public override void Load()
        {
        }

        // This method is called when the plugin is unloaded during the Loupedeck service shutdown.
        public override void Unload()
        {
        }
    }
}
