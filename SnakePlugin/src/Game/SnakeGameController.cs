namespace Loupedeck.SnakePlugin
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class SnakeGameController
    {
        private static readonly object _lock = new();
        private static readonly List<Action> _subscribers = new();
        private static readonly SnakeGame _game = new();

        private static CancellationTokenSource _cts;
        private static Task _loopTask;

        public static SnakeGame Game => _game;
        public static object Lock => _lock;

        public static void Subscribe(Action notify)
        {
            lock (_lock) _subscribers.Add(notify);
        }

        public static void Unsubscribe(Action notify)
        {
            lock (_lock) _subscribers.Remove(notify);
        }

        public static void StartLoop()
        {
            if (_loopTask != null && !_loopTask.IsCompleted)
                return; // already running

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _loopTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(500, token).ConfigureAwait(false);
                    if (token.IsCancellationRequested) break;
                    bool ticked;
                    lock (_lock)
                    {
                        ticked = _game.State == GameState.Playing;
                        _game.Tick();
                    }
                    if (ticked) NotifyAll();
                }
            }, token);
        }

        public static void StopLoop()
        {
            _cts?.Cancel();
            try { _loopTask?.Wait(500); } catch { }
        }

        // slotKey is "rowcol" e.g. "00", "11", "22"
        public static void HandlePress(string slotKey)
        {
            var row = slotKey[0] - '0';
            var col = slotKey[1] - '0';

            lock (_lock)
            {
                if (_game.State == GameState.GameOver)
                {
                    _game.Restart(); // reset to Paused, player presses centre to start again
                }
                else if (row == 1 && col == 1)
                {
                    // Center = start / pause / unpause
                    _game.TogglePause();
                }
                else
                {
                    var dir = (row, col) switch
                    {
                        (0, _) => Direction.Up,
                        (2, _) => Direction.Down,
                        (1, 0) => Direction.Left,
                        (1, 2) => Direction.Right,
                        _      => (Direction?)null
                    };
                    if (dir.HasValue)
                    {
                        _game.SetDirection(dir.Value);
                        // Direction press also unpauses — start moving
                        if (_game.State == GameState.Paused)
                            _game.TogglePause();
                    }
                }
            }
            NotifyAll();
        }

        private static void NotifyAll()
        {
            List<Action> snapshot;
            lock (_lock) snapshot = new List<Action>(_subscribers);
            foreach (var a in snapshot)
                try { a(); } catch { }
        }
    }
}
