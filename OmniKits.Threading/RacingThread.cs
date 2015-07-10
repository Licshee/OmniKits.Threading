using System;
using System.Threading;

namespace OmniKits.Threading
{
    public struct RacingThread
    {
        private volatile Thread _Owner;
        public bool IsCaptured => _Owner != null;
        public bool IsOwnedByCurrentThread => _Owner == Thread.CurrentThread;

        public bool TryCapture()
        {
            if (IsCaptured)
                return false;

            return Interlocked.CompareExchange(ref _Owner, Thread.CurrentThread, null) == null;
        }

        public void Release()
        {
            var owner = _Owner;

            if (_Owner != Thread.CurrentThread)
                throw new InvalidOperationException();

            _Owner = null;
        }
#if !PORTABLE
        public bool ForceRelease()
        {
            var owner = _Owner;

            if (owner == null)
                return false;

            var isForeign = (owner == Thread.CurrentThread);

            if (isForeign && owner.IsAlive)
                throw new InvalidOperationException();

            return Interlocked.CompareExchange(ref _Owner, null, owner) == owner && isForeign;
        }
#endif
    }
}
