using System;

namespace N3O.Umbraco.Lookups;

[AttributeUsage(AttributeTargets.Field)]
public class LookupInfoAttribute : Attribute {
    public LookupInfoAttribute(Type lookupType) {
        LookupType = lookupType;
    }
    
    public Type LookupType { get; }
}
