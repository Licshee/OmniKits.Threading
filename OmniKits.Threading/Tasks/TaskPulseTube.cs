using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmniKits.Threading.Tasks
{
    public class TaskPulseTube<T> : PulseTube<T>//, IEnumerable<Task<T>>
    {
        protected IAwaitData<Pair> CurrentTaskNode => (IAwaitData<Pair>)CurrentNode;

        protected override IDataPulse<Pair> CreateNode()
            => new TaskDataPulse<Pair>();

        protected class TaskEnumerator : IEnumerator<Task<T>>
        {
            IAwaitData<Pair> _Node;
            bool _NoData = true;
            Task<T> _Task;

            public TaskEnumerator(IAwaitData<Pair> node)
            {
                _Node = node;
            }

            public Task<T> Current
            {
                get
                {
                    if (_NoData)
                        throw new InvalidOperationException();
                    return _Task;
                }
            }
            object IEnumerator.Current => Current;

            public virtual void Dispose() { }

            public virtual bool MoveNext()
            {
                throw new NotImplementedException();

                //if (_Node == null)
                //    return false;

                ////var task = _Node.Task;
                ////_Task = Task<T>.Factory.StartNew(() => task.Result.Data);

                //var pair = _Node.Data;
                //_Node = (IWaitDataAsync<Pair>)pair.NextNode;

                //if (_Node == null)
                //    return false;

                //if (_NoData)
                //    _NoData = false;
            }

            public virtual void Reset()
            {
                throw new NotSupportedException();
            }
        }

        //public new IEnumerator<Task<T>> GetEnumerator()
        //    => new TaskEnumerator(CurrentTaskNode);
    }
}
