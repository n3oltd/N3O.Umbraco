using System;

namespace N3O.Umbraco.Hosting;

[AttributeUsage(AttributeTargets.Class)]
public class StringSchemaAttribute : Attribute {
    public string ApiDescription { get; }

    public StringSchemaAttribute(string apiDescription) {
        ApiDescription = apiDescription;
    }
}
