using System;
using System.Windows.Threading;

namespace Managing.Common
{
    /// <summary>
    /// Provides an ability to invoke custom action on the UI thread.
    /// </summary>
    public interface IDispatcherInvoker
    {
        /// <summary>
        /// Invokes the specified delegate on the UI thread synchronously.
        /// </summary>
        /// <param name="action">The delegate to be invoked.</param>
        void Invoke(Action action);

        /// <summary>
        /// Executes the specified <see cref="System.Action" /> asynchronously on the thread the <see cref="System.Windows.Threading.Dispatcher" /> is associated with.
        /// </summary>
        DispatcherOperation InvokeAsync(Action callback);

        /// <summary>
        /// Invokes the specified delegate on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The delegate to be invoked.</param>
        void BeginInvoke(Action action);
    }
}
