using System;

namespace OmniKits.Threading
{
    public abstract class DoubleCheckedLockingBase : LockingBase
    {
        protected DoubleCheckedLockingBase(bool lockSelf)
            : base(lockSelf)
        { }

        protected abstract bool IsLockingPermitted();

        protected override void TryLocked()
        {
            if (IsLockingPermitted())
                lock (LocalMonitor)
                    if (IsLockingPermitted())
                        LockedOperation();
        }
    }
}
