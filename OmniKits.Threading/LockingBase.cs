using System;

namespace OmniKits.Threading
{
    public abstract class LockingBase
    {
        protected object LocalMonitor { get; }

        protected LockingBase(bool lockSelf)
        {
            LocalMonitor = lockSelf ? this : new object();
        }

        protected abstract void LockedOperation();

        protected virtual void TryLocked()
        {
            lock (LocalMonitor)
                LockedOperation();
        }
    }
}
