using System;

namespace OmniKits
{
    public interface ICanLock
    {
        void TryLocked(Action action);
    }
}
