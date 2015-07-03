using System.Threading;
using System.Threading.Tasks;

namespace OmniKits
{
    public interface ICanRun
    {
        void Run();
    }

    public interface ICanRunAsync
    {
        Task RunAsync(CancellationToken cancellationToken);
    }

    public interface ICanRun<T>
    {
        T Run();
    }

    public interface ICanRunAsync<T>
    {
        Task<T> RunAsync(CancellationToken cancellationToken);
    }

    public interface ICanRunDuplex : ICanRun, ICanRunAsync
    { }

    public interface ICanRunDuplex<T> : ICanRun<T>, ICanRunAsync<T>
    { }

}
