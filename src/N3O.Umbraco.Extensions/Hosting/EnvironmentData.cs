using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting;

public static class EnvironmentData {
    private static readonly Dictionary<string, string> ConfigurationData = new(StringComparer.InvariantCultureIgnoreCase);
    private static readonly Dictionary<string, string> OverrideData = new(StringComparer.InvariantCultureIgnoreCase);
    
    public static void LoadFromConfiguration(IConfiguration configuration) {
        var section = configuration.GetSection("Environment");

        foreach (var (key, value) in section.AsEnumerable(true)) {
            ConfigurationData[key] = value;
        }
    }

    public static void Override(string key, string value) {
        OverrideData[key] = value;
    }
    
    public static void OverrideOur(string key, string value) {
        Override(GetOurKey(key), value);
    }
    
    public static string GetOurKey(string key) {
        return $"N3O_{key}";
    }
    
    public static string GetOurValue(string key, string defaultValue = null) {
        return GetValue(GetOurKey(key), defaultValue);
    }

    public static string GetValue(string key, string defaultValue = null) {
        if (OverrideData.TryGetValue(key, out var overrideValue)) {
            return overrideValue;
        } else if (ConfigurationData.TryGetValue(key, out var configurationValue)) {
            return configurationValue;
        } else {
            return Environment.GetEnvironmentVariable(key) ?? defaultValue;
        }
    }
}