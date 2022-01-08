using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Locks {
    public interface ILock {
        Task LockAsync(string name, Func<Task> action);
        Task<T> LockAsync<T>(string name, Func<Task<T>> action);
    }
}