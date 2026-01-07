using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Element : ContentOrPublishedLookup {
    public Element(string id, string name, Guid? contentId, ElementKind elementKind) : base(id, name, contentId) {
        ElementKind = elementKind;
    }
    
    public ElementKind ElementKind { get; }
}