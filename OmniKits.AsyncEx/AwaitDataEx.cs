namespace OmniKits.Threading.Tasks
{
#if NET45
    using System.Runtime.CompilerServices;
#else
    using Microsoft.Runtime.CompilerServices;
#endif

    public static class AwaitDataEx
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this IAwaitData<T> source)
            => source.Task.GetAwaiter();
    }
}
