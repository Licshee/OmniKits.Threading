using System.Threading.Tasks;

namespace OmniKits
{
    public interface IAwaitData<T> : IWaitData<T>
    {
        Task<T> Task { get; }
    }
}
