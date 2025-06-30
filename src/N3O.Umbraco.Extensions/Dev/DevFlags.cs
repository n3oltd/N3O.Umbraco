using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Dev;

public static class DevFlags {
    private static readonly HashSet<string> Flags = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Clear(string flag) {
        Flags.Remove(flag);
    }
    
    public static bool IsNotSet(string flag) {
        return !IsSet(flag);
    }
    
    public static bool IsSet(string flag) {
        return Flags.Contains(flag);
    }
    
    public static void Set(string flag) {
        Flags.Add(flag);
    }
}