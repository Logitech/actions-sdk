namespace Loupedeck.SnakePlugin
{
    using System;
    using System.Collections.Generic;

    public enum Direction { Up, Down, Left, Right }

    public enum GameState { Playing, Paused, GameOver }

    // Pure game state — no rendering, no threading concerns.
    // All public methods must be called under the caller's lock.
    public class SnakeGame
    {
        public const int GridSize = 9;

        private readonly LinkedList<(int x, int y)> _snake = new();
        private readonly Random _rng = new();

        public (int x, int y) Food { get; private set; }
        public GameState State { get; private set; }
        public int Score { get; private set; }
        public Direction CurrentDirection { get; private set; }
        private Direction _nextDirection;

        public IEnumerable<(int x, int y)> Snake => _snake;
        public (int x, int y) Head => _snake.First.Value;

        public SnakeGame()
        {
            this.Reset();
            this.State = GameState.Paused;
        }

        // Reset and stay paused — call after game over so player must press centre to restart
        public void Restart()
        {
            this.Reset();
            this.State = GameState.Paused;
        }

        public void Reset()
        {
            _snake.Clear();
            // Start left side, heading right — 6 cells of runway before hitting the wall
            _snake.AddFirst((2, 4));
            _snake.AddLast((1, 4));
            _snake.AddLast((0, 4));
            this.CurrentDirection = Direction.Right;
            _nextDirection = Direction.Right;
            this.State = GameState.Playing;
            this.Score = 0;
            this.SpawnFood();
        }

        public void SetDirection(Direction dir)
        {
            // Ignore 180° reversals
            if (dir == Direction.Up    && this.CurrentDirection == Direction.Down)  return;
            if (dir == Direction.Down  && this.CurrentDirection == Direction.Up)    return;
            if (dir == Direction.Left  && this.CurrentDirection == Direction.Right) return;
            if (dir == Direction.Right && this.CurrentDirection == Direction.Left)  return;
            _nextDirection = dir;
        }

        public void TogglePause()
        {
            if (this.State == GameState.Playing)      this.State = GameState.Paused;
            else if (this.State == GameState.Paused)  this.State = GameState.Playing;
        }

        public void Tick()
        {
            if (this.State != GameState.Playing) return;

            this.CurrentDirection = _nextDirection;

            var (hx, hy) = _snake.First.Value;
            var newHead = this.CurrentDirection switch
            {
                Direction.Up    => (hx,     hy - 1),
                Direction.Down  => (hx,     hy + 1),
                Direction.Left  => (hx - 1, hy),
                Direction.Right => (hx + 1, hy),
                _               => (hx,     hy)
            };

            // Wall collision
            if (newHead.Item1 < 0 || newHead.Item1 >= GridSize ||
                newHead.Item2 < 0 || newHead.Item2 >= GridSize)
            {
                this.State = GameState.GameOver;
                return;
            }

            // Self collision — check before adding head
            foreach (var seg in _snake)
            {
                if (seg == newHead)
                {
                    this.State = GameState.GameOver;
                    return;
                }
            }

            _snake.AddFirst(newHead);

            if (newHead == this.Food)
            {
                this.Score++;
                this.SpawnFood();
            }
            else
            {
                _snake.RemoveLast();
            }
        }

        private void SpawnFood()
        {
            var occupied = new HashSet<(int, int)>(_snake);
            var empty = new List<(int, int)>(GridSize * GridSize);
            for (var y = 0; y < GridSize; y++)
                for (var x = 0; x < GridSize; x++)
                    if (!occupied.Contains((x, y)))
                        empty.Add((x, y));

            this.Food = empty.Count > 0 ? empty[_rng.Next(empty.Count)] : (-1, -1);
        }
    }
}
