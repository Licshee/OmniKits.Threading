using Microsoft.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniKits
{
    public static class WaitDataAsyncEx
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this IWaitDataAsync<T> source)
            => source.Task.GetAwaiter();
    }
}
