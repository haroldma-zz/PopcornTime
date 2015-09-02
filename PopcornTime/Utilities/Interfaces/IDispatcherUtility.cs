using System;
using System.Threading.Tasks;

namespace PopcornTime.Utilities.Interfaces
{
    public interface IDispatcherUtility
    {
        void Run(Action action);
        T Run<T>(Func<T> func);
        Task RunAsync(Action action);
        Task<T> RunAsync<T>(Func<Task<T>> func);
        Task<T> RunAsync<T>(Func<T> func);
    }
}