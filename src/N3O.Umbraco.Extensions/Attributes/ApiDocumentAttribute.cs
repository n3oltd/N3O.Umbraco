using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiDocumentAttribute : Attribute {
    public ApiDocumentAttribute(string apiName) {
        ApiName = apiName;
    }
    
    public string ApiName { get; }
}
