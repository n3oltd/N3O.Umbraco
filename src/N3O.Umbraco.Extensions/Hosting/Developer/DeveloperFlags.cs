using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting;

public static class DeveloperFlags {
    private static readonly HashSet<string> Flags = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Add(string flag) {
        Flags.Add(flag);
    }
    
    public static void Remove(string flag) {
        Flags.Remove(flag);
    }

    public static bool IsNotSet(string flag) {
        return !IsSet(flag);
    }
    
    public static bool IsSet(string flag) {
        return Flags.Contains(flag);
    }
}