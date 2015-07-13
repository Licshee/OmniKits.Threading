using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniKits.Threading.Tasks
{
    public abstract class MutexCommandBase<T>
    {
        protected object LocalMonitor { get; }
        private Task<T> _Task;
        protected Task<T> CurrentTask => _Task;

        protected MutexCommandBase(bool lockSelf)
        {
            LocalMonitor = lockSelf ? this : new object();
        }

        protected abstract Task<T> MainAsync();

        private Task<T> GetTaskMutex()
        {
            {
                var task = _Task;
                if (!(task == null || task.IsCompleted))
                    return task;
            }
            lock (LocalMonitor)
            {
                var task = _Task;
                if (!(task == null || task.IsCompleted))
                    return task;

                return MainAsync();
            }
        }

        public T Run()
            => GetTaskMutex().Result;
        public Task RunAsync()
            => GetTaskMutex();
    }
}
