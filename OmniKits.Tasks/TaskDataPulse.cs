using System.Threading.Tasks;

namespace OmniKits.Threading.Tasks
{
    public sealed class TaskDataPulse<T> : ITaskDataPulse<T>
    {
        TaskCompletionSource<T> _TCS = new TaskCompletionSource<T>();
        public Task<T> Task { get; }

        public TaskDataPulse()
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
