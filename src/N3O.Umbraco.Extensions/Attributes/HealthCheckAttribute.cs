using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HealthCheckAttribute : Attribute {
    public HealthCheckAttribute(string name) {
        Name = name;
    }

    public string Name { get; }
}
