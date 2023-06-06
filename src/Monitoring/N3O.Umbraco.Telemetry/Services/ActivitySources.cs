using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry; 

public static class ActivitySources {
    private static readonly ConcurrentDictionary<Type, ActivitySource> Sources = new();

    public static ActivitySource Get<T>() {
        return Get(typeof(T));
    }
    
    public static ActivitySource Get(Type type) {
        return Sources.GetOrAdd(type, k => new ActivitySource(k.FullName));
    }
}