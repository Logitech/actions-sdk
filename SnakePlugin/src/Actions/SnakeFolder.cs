namespace Loupedeck.SnakePlugin
{
    using System;
    using System.Collections.Generic;

    public class SnakeFolder : PluginDynamicFolder
    {
        // 9 slots, row-major: "00".."22"  (row,col)
        private static readonly string[] SlotNames = new[]
        {
            "00", "01", "02",
            "10", "11", "12",
            "20", "21", "22",
        };

        // Pre-rendered tile cache — all 9 tiles rendered atomically from same game snapshot
        private readonly BitmapImage[] _tileCache = new BitmapImage[9];

        public SnakeFolder()
        {
            this.DisplayName = "Play Snake";
            this.Description  = "Press to play Snake.";
            this.GroupName    = "Games";
        }

        // ── lifecycle ────────────────────────────────────────────────────────

        public override Boolean Load()
        {
            PluginLog.Info("SnakeFolder: Load");
            SnakeGameController.Subscribe(this.OnGameStateChanged);
            SnakeGameController.StartLoop();
            return true;
        }

        public override Boolean Unload()
        {
            PluginLog.Info("SnakeFolder: Unload");
            SnakeGameController.Unsubscribe(this.OnGameStateChanged);
            SnakeGameController.StopLoop();
            return true;
        }

        private void OnGameStateChanged()
        {
            // Render all 9 tiles in a single lock so every tile sees the same game snapshot
            lock (SnakeGameController.Lock)
            {
                var game = SnakeGameController.Game;
                for (var row = 0; row < 3; row++)
                    for (var col = 0; col < 3; col++)
                        _tileCache[row * 3 + col] = SnakeRenderer.RenderTile(116, row, col, game);
            }

            foreach (var slot in SlotNames)
                this.CommandImageChanged(slot);
        }

        // ── slot layout ───────────────────────────────────────────────────────

        public override PluginDynamicFolderNavigation GetNavigationArea(DeviceType deviceType)
            => PluginDynamicFolderNavigation.EncoderArea;

        public override IEnumerable<String> GetButtonPressActionNames(DeviceType deviceType)
        {
            var names = new List<String>();
            foreach (var s in SlotNames)
                names.Add(this.CreateCommandName(s));
            return names;
        }

        // ── rendering ─────────────────────────────────────────────────────────

        public override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
            => "";

        public override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            try
            {
                var key = SlotKey(actionParameter);
                int row = key[0] - '0';
                int col = key[1] - '0';
                int idx = row * 3 + col;

                // Return cached tile if available, otherwise render on demand
                var cached = _tileCache[idx];
                if (cached != null) return cached;

                lock (SnakeGameController.Lock)
                    return SnakeRenderer.RenderTile(TilePixels(imageSize), row, col, SnakeGameController.Game);
            }
            catch (Exception ex)
            {
                PluginLog.Error($"SnakeFolder.GetCommandImage: {ex.Message}");
                return null;
            }
        }

        // ── entry button ──────────────────────────────────────────────────────

        public override String GetButtonDisplayName(PluginImageSize imageSize) => "";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            try
            {
                int size = TilePixels(imageSize);
                var icon = PluginResources.ReadImage("snake-icon.png");
                using var b = new BitmapBuilder(size, size);
                b.Clear(new BitmapColor(20, 20, 40));
                b.DrawImage(icon, 0, 0, size, size);
                return b.ToImage();
            }
            catch (Exception ex)
            {
                PluginLog.Error($"SnakeFolder.GetButtonImage: {ex.Message}");
                int size = TilePixels(imageSize);
                using var b = new BitmapBuilder(size, size);
                b.Clear(new BitmapColor(0, 80, 0));
                b.DrawText("SNAKE", 0, 0, size, size, BitmapColor.White, 14);
                return b.ToImage();
            }
        }

        // ── input ─────────────────────────────────────────────────────────────

        public override void RunCommand(String actionParameter)
        {
            var key = SlotKey(actionParameter);
            PluginLog.Info($"SnakeFolder: RunCommand key={key}");

            // Corners: (00) = EXIT on pause/game over; others are always no-ops
            if (key == "00")
            {
                lock (SnakeGameController.Lock)
                {
                    var state = SnakeGameController.Game.State;
                    if (state == GameState.Paused || state == GameState.GameOver)
                        this.Close();
                }
                return;
            }
            if (key == "02" || key == "20" || key == "22")
                return;

            SnakeGameController.HandlePress(key);
        }

        // ── helpers ───────────────────────────────────────────────────────────

        // Extract the "rowcol" suffix from the full command name e.g. "SnakeFolder/00" → "00"
        private static String SlotKey(String actionParameter) =>
            actionParameter.Length >= 2
                ? actionParameter.Substring(actionParameter.Length - 2)
                : actionParameter;

        private static int TilePixels(PluginImageSize s) => s switch
        {
            PluginImageSize.Width116Pixels => 116,
            PluginImageSize.Width90Pixels  => 80,
            PluginImageSize.Width116       => 116,
            PluginImageSize.Width90        => 80,
            _                              => 116
        };
    }
}
