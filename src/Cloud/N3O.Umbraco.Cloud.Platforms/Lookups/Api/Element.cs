using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Element : ContentOrPublishedLookup {
    public Element(string id,
                   string name,
                   Guid? contentId,
                   ElementKind elementKind,
                   string embedCode) : base(id, name, contentId) {
        ElementKind = elementKind;
        EmbedCode = embedCode;
    }
    
    public ElementKind ElementKind { get; }
    public string EmbedCode { get; }
}