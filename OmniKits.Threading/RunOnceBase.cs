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

        private volatile Task<T> _CurrentTask;
        public Task<T> CurrentTask => _CurrentTask;

        public RunOnceBase(bool lockSelf)
        {
            LocalMonitor = lockSelf ? this : new object();
        }

        protected abstract Task<T> MainAsync();

        public virtual Task<T> RunAsync()
        {
            var task = CurrentTask;
            if (task != null)
                return task;

            lock (LocalMonitor)
            {
                if (task != null)
                    return task;

                task = MainAsync();
                _CurrentTask = task;
            }
            return task;
        }
    }
}
