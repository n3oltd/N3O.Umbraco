using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ElementType : NamedLookup {
    public ElementType(string id, string name, string tagName, string contentTypeAlias) : base(id, name) {
        TagName = tagName;
        ContentTypeAlias = contentTypeAlias;
    }
    
    public string TagName { get; }
    public string ContentTypeAlias { get; }
  
    public string GetEmbedCode(Guid id) {
        var tag = new TagBuilder(TagName);
        
        tag.Attributes.Add("id", id.ToString("N"));

        return tag.ToHtmlString();
    }
}

public class ElementTypes : StaticLookupsCollection<ElementType> {
    public static readonly ElementType DonateButton = new("donateButton",
                                                          "Donate Button",
                                                          "n3o-donate-button",
                                                          PlatformsConstants.Elements.DonateButton);

    public static readonly ElementType DonationForm = new("donationForm",
                                                          "Donation Form",
                                                          "n3o-donation-form",
                                                          PlatformsConstants.Elements.DonationForm);
    public static ElementType FindByContentTypeAlias(string contentTypeAlias) {
        return StaticLookups.GetAll<ElementType>()
                            .SingleOrDefault(x => x.ContentTypeAlias.EqualsInvariant(contentTypeAlias));
    }
}