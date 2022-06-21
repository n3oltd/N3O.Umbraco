using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Locks;

public interface ILocker {
    IDisposable Lock(string key);
    Task<IDisposable> LockAsync(string key);
}
