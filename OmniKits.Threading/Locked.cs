using System;

namespace OmniKits.Threading
{
    public class Locked : ICanLock
    {
        protected object LocalMonitor { get; }

        public Locked(bool lockSelf)
        {
            LocalMonitor = lockSelf ? this : new object();
        }

        public virtual void TryLocked(Action action)
        {
            lock (LocalMonitor)
                action();
        }
    }
}
