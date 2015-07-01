using System;

namespace OmniKits.Threading
{
    public abstract class DoubleCheckedLockBase : Locked
    {
        public DoubleCheckedLockBase(bool lockSelf)
            : base(lockSelf)
        { }

        protected abstract bool IsLockingPermitted();

        public override void TryLocked(Action action)
        {
            if (IsLockingPermitted())
                lock (LocalMonitor)
                    if (IsLockingPermitted())
                        action();
        }
    }
}
