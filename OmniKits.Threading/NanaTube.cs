using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace OmniKits.Threading
{
    public class NanaTube<T> : IEnumerable<T>
    {
        IWaitData<Pair> _Node;
        protected IWaitData<Pair> CurrentNode => _Node;

        public NanaTube()
        {
            _Node = CreateNode();
        }

        protected struct Pair
        {
            public IWaitData<Pair> NextNode;
            public T Data;

            public Pair(IWaitData<Pair> node, T data)
            {
                NextNode = node;
                Data = data;
            }
        }

        protected virtual IWaitData<Pair> CreateNode()
            => new DataTrigger<Pair>();

        public virtual void Push(T data)
        {
            var node = CreateNode();
            var old = Interlocked.Exchange(ref _Node, node);
            old.Data = new Pair(node, data);
        }

        protected class InnerEnumerator : IEnumerator<T>
        {
            IWaitData<Pair> _Node;
            bool _NoData = true;
            T _Data;

            public InnerEnumerator(IWaitData<Pair> node)
            {
                _Node = node;
            }

            public T Current
            {
                get
                {
                    if (_NoData)
                        throw new InvalidOperationException();
                    return _Data;
                }
            }
            object IEnumerator.Current => Current;

            public virtual void Dispose() { }

            public virtual bool MoveNext()
            {
                if (_Node == null)
                    return false;

                var pair = _Node.Data;
                _Node = pair.NextNode;

                if (_Node == null)
                    return false;

                if (_NoData)
                    _NoData = false;

                _Data = pair.Data;
                return true;
            }

            public virtual void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public virtual IEnumerator<T> GetEnumerator()
            => new InnerEnumerator(CurrentNode);
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public virtual void Break()
        {
            var old = Interlocked.Exchange(ref _Node, null);
            old.Data = new Pair();
        }
    }
}
