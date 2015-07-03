using System;
using System.Threading;

namespace OmniKits.Threading
{
    public sealed class DataPulse<T> : IWaitData<T>
    {
        class Entry
        {
            public T Data;
        }

        volatile ManualResetEvent _MRE = new ManualResetEvent(false); // TODO: check if this volatile is really necessary
        Entry _Entry; // no chance to read an uninitialized entry, so no need to mark it volatile

        public T Data
        {
            get
            {
                var mre = _MRE;
                if (mre != null)
                {
                    try
                    {
                        mre.WaitOne();
                    }
                    catch { }
                }
                return _Entry.Data;
            }

            set
            {
                var entry = new Entry { Data = value };
                var old = Interlocked.CompareExchange(ref _Entry, entry, null);
                if (old != null)
                    throw new InvalidOperationException();

                var mre = _MRE;
                _MRE = null;
                mre.Set();
                mre.Dispose();
            }
        }
    }
}
