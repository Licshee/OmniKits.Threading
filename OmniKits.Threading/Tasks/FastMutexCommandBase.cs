using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniKits.Threading.Tasks
{
    public abstract class FastMutexCommandBase<T>
    {
        private TaskCompletionSource<Task<T>> _TCS;
        private Task<T> _Task;
        private volatile int _State = 0;

        protected FastMutexCommandBase()
        {
            _TCS = new TaskCompletionSource<Task<T>>();
            _Task = _TCS.Task.Unwrap();
        }

        protected abstract Task<T> MainAsync();

        private Task<T> GetTaskMutex()
        {
            throw new NotImplementedException();
        }

        public T Run()
            => GetTaskMutex().Result;
        public Task RunAsync()
            => GetTaskMutex();
    }
}
