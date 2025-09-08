using System;

namespace N3O.Umbraco.Sync.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DataSyncProviderAttribute : Attribute {
    public DataSyncProviderAttribute(string id) {
        Id = id;
    }
    
    public string Id { get; }
}