using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniKits.Threading
{
    public abstract class RunOnceBase<T>
    {
        protected object LocalMonitor { get; }

        class AtomicState
        {
            public CancellationTokenSource CTS;
            public Task<T> Task;
        }
        private volatile AtomicState _State;
        public Task<T> CurrentTask => _State.Task;

        protected RunOnceBase(bool lockSelf)
        {
            LocalMonitor = lockSelf ? this : new object();
        }

        protected abstract Task<T> MainAsync(CancellationToken cancellationToken);

        public Task<T> RunAsync()
        {
            {
                var state = _State;
                if (state != null)
                    return state.Task;
            }
            lock (LocalMonitor)
            {
                var state = _State;
                if (state != null)
                    return state.Task;

                var cts = new CancellationTokenSource();
                _State = new AtomicState
                {
                    CTS = cts,
                    Task = MainAsync(cts.Token),
                };
                return _State.Task;
            }
        }
        public T Run()
            => RunAsync().Result;

        protected void Reset(bool forceReset)
        {
            {
                var state = _State;
                if (state == null)
                    return;
                if (!(forceReset || state.CTS.IsCancellationRequested))
                    throw new InvalidOperationException();
            }
            lock (LocalMonitor)
            {
                var state = _State;
                if (state == null)
                    return;

                if (!state.CTS.IsCancellationRequested)
                {
                    if (forceReset)
                        state.CTS.Cancel();
                }

                _State = null;
            }
        }
        public void Reset()
            => Reset(false);

        public void Stop()
        {
            {
                var state = _State;
                if (state == null || state.CTS.IsCancellationRequested)
                    throw new InvalidOperationException();
            }
            lock (LocalMonitor)
            {
                var state = _State;
                if (state == null || state.CTS.IsCancellationRequested)
                    throw new InvalidOperationException();

                state.CTS.Cancel();
            }
        }
    }
}
