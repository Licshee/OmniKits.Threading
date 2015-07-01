using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniKits.Threading
{
    public sealed class DataTriggerAsync<T> : IWaitDataAsync<T>
    {
        TaskCompletionSource<T> _TCS = new TaskCompletionSource<T>();
        public Task<T> Task { get; }

        public DataTriggerAsync()
        {
            Task = _TCS.Task;
        }

        public T Data
        {
            get
            {
                return Task.Result;
            }

            set
            {
                _TCS.SetResult(value);
            }
        }
    }
}
