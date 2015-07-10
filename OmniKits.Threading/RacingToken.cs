using System;
using System.Threading;

namespace OmniKits.Threading
{
    public struct RacingToken<T>
        where T : class
    {
        private volatile T _Token;
        public bool IsCaptured => _Token != null;
        public T Token => _Token;

        public bool TryCapture(T token)
        {
            if (token == null)
                throw new ArgumentNullException();

            if (IsCaptured)
                return false;

            return Interlocked.CompareExchange(ref _Token, token, null) == null;
        }

        public bool TryRelease(T token)
        {
            if (token == null)
                throw new ArgumentNullException();

            if (!IsCaptured)
                return false;

            return Interlocked.CompareExchange(ref _Token, null, token) == token;
        }
    }
}
