using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HealthCheckAttribute : Attribute {
    public HealthCheckAttribute(string name, params string[] tags) {
        Name = name;
        Tags = tags ?? Array.Empty<string>();
    }

    public string Name { get; }
    public string[] Tags { get; }
}