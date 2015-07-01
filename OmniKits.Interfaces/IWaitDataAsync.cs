using System.Threading.Tasks;

namespace OmniKits
{
    public interface IWaitDataAsync<T> : IWaitData<T>
    {
        Task<T> Task { get; }
    }
}
