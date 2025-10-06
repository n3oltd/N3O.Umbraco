using System;

namespace N3O.Umbraco.Sync.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SyncOnPublishAttribute : Attribute {
    public SyncOnPublishAttribute(string serverAlias) {
        ServerAlias = serverAlias;
    }
    
    public string ServerAlias { get; }
}