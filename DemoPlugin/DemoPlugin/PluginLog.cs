using System;

namespace Loupedeck.DemoPlugin
{
    // A helper class to write to the plugin log.
    internal static class PluginLog
    {
        private static PluginLogFile pluginLogFile;

        public static void Init(PluginLogFile pluginLogFile)
        {
            pluginLogFile.CheckNullArgument(nameof(pluginLogFile));
            PluginLog.pluginLogFile = pluginLogFile;
        }

        public static void Verbose(string text) => pluginLogFile?.Verbose(text);

        public static void Verbose(Exception ex, string text) => pluginLogFile?.Verbose(ex, text);

        public static void Info(string text) => pluginLogFile?.Info(text);

        public static void Info(Exception ex, string text) => pluginLogFile?.Info(ex, text);

        public static void Warning(string text) => pluginLogFile?.Warning(text);

        public static void Warning(Exception ex, string text) => pluginLogFile?.Warning(ex, text);

        public static void Error(string text) => pluginLogFile?.Error(text);

        public static void Error(Exception ex, string text) => pluginLogFile?.Error(ex, text);
    }
}
