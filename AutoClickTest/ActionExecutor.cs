using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClickTest
{
    /// <summary>
    /// Executes the existing Action (Model.Action) list in background with Start/Pause/Resume/Stop support.
    /// It re-uses the project's existing Mouse and Keybord helpers.
    /// </summary>
    public class ActionExecutor
    {
        private readonly List<Action> _actions;
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);
        private Task _runningTask;

        public event Action<string> OnStatus;

        /// <summary>
        /// How many times to repeat the whole action sequence. 1 = run once.
        /// </summary>
        public int SequenceRepeat { get; set; } = 1;

        public ActionExecutor(List<Action> actions)
        {
            _actions = actions ?? new List<Action>();
        }

        public void Start()
        {
            if (_runningTask != null && !_runningTask.IsCompleted) return;
            _cts = new CancellationTokenSource();
            _pauseEvent.Set();
            _runningTask = Task.Run(() => RunLoop(_cts.Token));
        }

        public void Pause()
        {
            _pauseEvent.Reset();
            OnStatus?.Invoke("Paused");
        }

        public void Resume()
        {
            _pauseEvent.Set();
            OnStatus?.Invoke("Resumed");
        }

        public void Stop()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
            _pauseEvent.Set();
            OnStatus?.Invoke("Stopped");
        }

        private void RunLoop(CancellationToken token)
        {
            try
            {
                for (int seq = 0; seq < Math.Max(1, SequenceRepeat); seq++)
                {
                    OnStatus?.Invoke($"Sequence {seq + 1}/{SequenceRepeat} start");

                    for (int i = 0; i < _actions.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        _pauseEvent.Wait(token);

                        var a = _actions[i];
                        OnStatus?.Invoke($"Action {i + 1}/{_actions.Count}: {a?.Action_Desc}");

                        // wait for the configured delay (Delay_MS)
                        DelayWithCancel(a?.Delay_MS ?? 0, token);

                        if (a is KeyCodeAction key)
                        {
                            ExecuteKeyAction(key, token);
                        }
                        else if (a is MouseAction mouse)
                        {
                            ExecuteMouseAction(mouse, token);
                        }

                        // small pause between actions
                        DelayWithCancel(10, token);
                    }

                    OnStatus?.Invoke($"Sequence {seq + 1}/{SequenceRepeat} finished");
                }

                OnStatus?.Invoke("All sequences finished");
            }
            catch (OperationCanceledException)
            {
                OnStatus?.Invoke("Execution cancelled");
            }
            catch (Exception ex)
            {
                OnStatus?.Invoke("Error: " + ex.Message);
            }
        }

        private void DelayWithCancel(int ms, CancellationToken token)
        {
            if (ms <= 0) return;
            const int chunk = 100;
            int left = ms;
            while (left > 0)
            {
                token.ThrowIfCancellationRequested();
                _pauseEvent.Wait(token);
                int step = Math.Min(chunk, left);
                Task.Delay(step, token).Wait(token);
                left -= step;
            }
        }

        private void ExecuteKeyAction(KeyCodeAction key, CancellationToken token)
        {
            if (key == null) return;
            switch (key.Action_Desc)
            {
                case "按一下":
                    Keybord.Press(key.KeyCode?.ToUpper());
                    break;
                case "按住":
                    // Use Delay_MS as hold duration if provided, otherwise default 5000ms
                    int hold = Math.Max(0, key.Delay_MS > 0 ? key.Delay_MS : 5000);
                    Keybord.Hold(key.KeyCode?.ToUpper(), hold);
                    break;
                default:
                    break;
            }
        }

        private void ExecuteMouseAction(MouseAction mouse, CancellationToken token)
        {
            if (mouse == null) return;
            var rnd = new Random();
            switch (mouse.Action_Desc)
            {
                case "點一下左鍵":
                    Mouse.Move(mouse.X + rnd.Next(-10, 10), mouse.Y + rnd.Next(-10, 10));
                    Mouse.LeftClick();
                    break;
                case "點兩下左鍵":
                    Mouse.Move(mouse.X + rnd.Next(-10, 10), mouse.Y + rnd.Next(-10, 10));
                    Mouse.LeftClick();
                    Thread.Sleep(50);
                    Mouse.LeftClick();
                    break;
                case "點一下右鍵":
                    Mouse.Move(mouse.X + rnd.Next(-10, 10), mouse.Y + rnd.Next(-10, 10));
                    Mouse.RightClick();
                    break;
                case "點一下中鍵":
                    Mouse.Move(mouse.X + rnd.Next(-10, 10), mouse.Y + rnd.Next(-10, 10));
                    Mouse.MiddleClick();
                    break;
                case "前滾":
                    for (int j = 0; j < mouse.X; j++)
                    {
                        Mouse.Roll(Mouse.ROLLWAY.FRONT);
                    }
                    break;
                case "後滾":
                    for (int j = 0; j < mouse.X; j++)
                    {
                        Mouse.Roll(Mouse.ROLLWAY.BACK);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
