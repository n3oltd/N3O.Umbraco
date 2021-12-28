using System;

namespace N3O.Umbraco.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class UmbracoContentAttribute : Attribute {
        public UmbracoContentAttribute(string contentTypeAlias) {
            ContentTypeAlias = contentTypeAlias;
        }
    
        public string ContentTypeAlias { get; }
    }
}