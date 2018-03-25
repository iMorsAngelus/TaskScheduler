using System;
using System.Windows.Threading;

namespace Managing.Common
{
    /// <summary>
    /// Represents a thread invoker hosted on current Dispatcher thread.
    /// </summary>
    internal class WpfUiThreadInvoker : IDispatcherInvoker
    {
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        /// <inheritdoc cref="Invoke"/>.
        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        /// <inheritdoc />
        public DispatcherOperation InvokeAsync(Action callback)
        {
            return _dispatcher.InvokeAsync(callback);
        }

        /// <inheritdoc cref="BeginInvoke"/>.
        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}