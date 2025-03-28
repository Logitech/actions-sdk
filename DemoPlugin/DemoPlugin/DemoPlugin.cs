using System;

namespace Loupedeck.DemoPlugin
{
    public class DemoPlugin : Plugin
    {
        // Gets a value indicating whether this is an API-only plugin.
        public override bool UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is a Universal plugin or an Application plugin.
        public override bool HasNoApplication => true;

        // Initializes a new instance of the plugin class.
        public DemoPlugin()
        {
            // Initialize the plugin log.
            PluginLog.Init(base.Log);

            // Initialize the plugin resources.
            PluginResources.Init(base.Assembly);
        }

        // This method is called when the plugin is loaded.
        public override void Load()
        {
        }

        // This method is called when the plugin is unloaded.
        public override void Unload()
        {
        }
    }
}
