using System;

namespace N3O.Umbraco.Lookups {
    [AttributeUsage(AttributeTargets.Property)]
    public class FromLookupTypeAttribute : Attribute {
        public FromLookupTypeAttribute(string lookupTypeId) {
            LookupTypeId = lookupTypeId;
        }
        
        public string LookupTypeId { get; }
    }
}