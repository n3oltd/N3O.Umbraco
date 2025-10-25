using System;

namespace N3O.Umbraco.EditorJs.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TuneAttribute : Attribute {
    public TuneAttribute(string id) {
        Id = id;
    }

    public string Id { get; }
}