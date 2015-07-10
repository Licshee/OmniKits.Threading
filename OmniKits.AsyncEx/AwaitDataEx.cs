namespace OmniKits.Threading.Tasks
{
#if LegacyTask
    using Microsoft.Runtime.CompilerServices;
#else
    using System.Runtime.CompilerServices;
#endif

    public static class AwaitDataEx
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this IAwaitData<T> source)
            => source.Task.GetAwaiter();
    }
}
