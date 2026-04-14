using AsyncKeyedLock;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Extensions;

public static class AsyncKeyedLockerExtensions {
    public static void ExecuteLocked(this AsyncKeyedLocker<string> locker, Action action, params object[] values) {
        ExecuteLocked<None>(locker, () => {
            action();
            
            return None.Empty;
        }, values);
    }

    public static T ExecuteLocked<T>(this AsyncKeyedLocker<string> locker, Func<T> action, params object[] values) {
        using (locker.Lock(LockKey.Generate<T>(values))) {
            var result = action();

            return result;
        }
    }
}
