using System;
using System.Threading;
using System.Threading.Tasks;

namespace OmniKits.Threading.Tasks
{
    public abstract class DuplexCommandBase : ICanRunDuplex
    {
        private bool _IsRunOverridden = true;
        private bool _IsRunAsyncOverridden = true;

        private void EnsureOverridden()
        {
            if (_IsRunOverridden || _IsRunAsyncOverridden)
                return;
            throw new NotImplementedException();
        }

        public virtual void Run()
        {
            _IsRunOverridden = false;
            EnsureOverridden();

            Task.Factory.StartNew(() => RunAsync(CancellationToken.None)).Unwrap().Wait();
            //TaskEx.Run(() => RunAsync(CancellationToken.None)).Wait();
        }

        public virtual Task RunAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
                throw new NotSupportedException();

            _IsRunAsyncOverridden = false;
            EnsureOverridden();

            return Task.Factory.StartNew(Run);
        }
    }

    public abstract class DuplexCommandBase<T> : ICanRunDuplex<T>
    {
        private bool _IsRunOverridden = true;
        private bool _IsRunAsyncOverridden = true;

        private void EnsureOverridden()
        {
            if (_IsRunOverridden || _IsRunAsyncOverridden)
                return;
            throw new NotImplementedException();
        }

        public virtual T Run()
        {
            _IsRunOverridden = false;
            EnsureOverridden();

            return Task.Factory.StartNew(() => RunAsync(CancellationToken.None)).Unwrap().Result;
            //return TaskEx.Run(() => RunAsync(CancellationToken.None)).Result;
        }

        public virtual Task<T> RunAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
                throw new NotSupportedException();

            _IsRunAsyncOverridden = false;
            EnsureOverridden();

            return Task<T>.Factory.StartNew(Run);
        }
    }
}
