using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using PopcornTime.Common;
using PopcornTime.Utilities.Interfaces;

namespace PopcornTime.Utilities.RunTime
{
    public class DispatcherUtility : IDispatcherUtility
    {
        private readonly CoreDispatcher _coreDispatcher;

        public DispatcherUtility(CoreDispatcher coreDispatcher)
        {
            _coreDispatcher = coreDispatcher;
        }

        public void Run(Action action)
        {
            if (_coreDispatcher.HasThreadAccess)
                action();
            else
                AsyncHelper.RunSync(
                    () => _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action)).AsTask());
        }

        public T Run<T>(Func<T> func)
        {
            var obj = default(T);
            Run(() => { obj = func(); });
            return obj;
        }

        public Task RunAsync(Action action)
        {
            if (_coreDispatcher.HasThreadAccess)
            {
                action();
                return Task.FromResult(0);
            }
            return _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action)).AsTask();
        }

        public async Task<T> RunAsync<T>(Func<T> func)
        {
            var obj = default(T);
            await RunAsync(() => { obj = func(); });
            return obj;
        }

        public Task<T> RunAsync<T>(Func<Task<T>> func)
        {
            var src = new TaskCompletionSource<T>();
#pragma warning disable 4014
            RunAsync(async () =>
#pragma warning restore 4014
            {
                try
                {
                    src.SetResult(await func());
                }
                catch (Exception e)
                {
                    src.SetException(e);
                }
            });
            return src.Task;
        }
    }
}