using System;
using System.Threading;

namespace OmniKits.Threading
{
    public sealed class DataPulse<T> : IDataPulse<T>
    {
        volatile ManualResetEvent _MRE = new ManualResetEvent(false); // TODO: check if this volatile is really necessary
        int _HasData = 0;
        T _Data; // no chance to read an uninitialized entry, so no need to mark it volatile

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
                return _Data;
            }

            set
            {
                if(Interlocked.CompareExchange(ref _HasData, 1, 0) != 0)
                    throw new InvalidOperationException();

                _Data = value;

                var mre = _MRE;
                _MRE = null;
                mre.Set();
                mre.Dispose();
            }
        }
    }
}
