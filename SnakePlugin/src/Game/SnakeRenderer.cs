namespace Loupedeck.SnakePlugin
{
    using System.Collections.Generic;

    public static class SnakeRenderer
    {
        private static readonly BitmapColor BgColor   = new BitmapColor(20,  20,  40);
        private static readonly BitmapColor GridColor = new BitmapColor(35,  35,  60);
        private static readonly BitmapColor HeadColor = new BitmapColor(139, 195, 74);
        private static readonly BitmapColor BodyColor = new BitmapColor(76,  175, 80);
        private static readonly BitmapColor FoodColor = new BitmapColor(255, 68,  68);

        // Each tile (tileRow, tileCol) covers game cells:
        //   x: tileCol*3 .. tileCol*3+2
        //   y: tileRow*3 .. tileRow*3+2
        // Cell size within the tile: tileSize / 3
        public static BitmapImage RenderTile(int tileSize, int tileRow, int tileCol, SnakeGame game)
        {
            int cell = tileSize / 3;          // pixels per game cell (~38 for 116px tile)
            int startX = tileCol * 3;
            int startY = tileRow * 3;

            using var b = new BitmapBuilder(tileSize, tileSize);
            b.Clear(BgColor);

            // Faint grid lines
            for (var i = 1; i < 3; i++)
            {
                b.FillRectangle(i * cell, 0, 1, tileSize, GridColor);
                b.FillRectangle(0, i * cell, tileSize, 1, GridColor);
            }

            // Build a set of snake positions for fast lookup
            var snakeSet = new HashSet<(int, int)>(game.Snake);
            var head = game.Head;

            // Draw food if it falls in this tile
            var (fx, fy) = game.Food;
            if (fx >= startX && fx < startX + 3 && fy >= startY && fy < startY + 3)
            {
                int lx = (fx - startX) * cell;
                int ly = (fy - startY) * cell;
                b.FillRectangle(lx + 2, ly + 2, cell - 4, cell - 4, FoodColor);
            }

            // Draw snake cells that fall in this tile
            foreach (var (sx, sy) in game.Snake)
            {
                if (sx < startX || sx >= startX + 3 || sy < startY || sy >= startY + 3)
                    continue;

                int lx = (sx - startX) * cell;
                int ly = (sy - startY) * cell;
                var color = (sx == head.x && sy == head.y) ? HeadColor : BodyColor;
                b.FillRectangle(lx + 1, ly + 1, cell - 2, cell - 2, color);
            }

            // Overlay on center tile (1,1): status text
            if (tileRow == 1 && tileCol == 1)
            {
                if (game.State == GameState.Paused)
                    b.DrawText("PAUSED", 0, 0, tileSize, tileSize, BitmapColor.White, 12);
                else if (game.State == GameState.GameOver)
                    b.DrawText($"SCORE\n{game.Score}", 0, 0, tileSize, tileSize, BitmapColor.White, 12);
            }

            // Overlay on top-left tile (0,0): EXIT button when paused or game over
            if (tileRow == 0 && tileCol == 0 && game.State != GameState.Playing)
                b.DrawText("EXIT", 0, 0, tileSize, tileSize, BitmapColor.White, 14);

            return b.ToImage();
        }
    }
}
