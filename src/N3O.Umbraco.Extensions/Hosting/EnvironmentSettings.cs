using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting;

public static class EnvironmentSettings {
    private static readonly Dictionary<string, string> ConfigValues = new(StringComparer.InvariantCultureIgnoreCase);
    
    public static void LoadFromConfiguration(IConfiguration configuration) {
        var section = configuration.GetSection("Environment");

        foreach (var (key, value) in section.AsEnumerable(true)) {
            ConfigValues[key] = value;
        }
    }

    public static string GetValue(string key, string defaultValue = null) {
        var value = Environment.GetEnvironmentVariable(key) ?? ConfigValues.GetValueOrDefault(key, defaultValue);

        return value;
    }
}