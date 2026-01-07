using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Campaign : ContentOrPublishedLookup {
    public Campaign(string id, string name, Guid? contentId) : base(id, name, contentId) { }

    public string GetDonationFormEmbedCode() {
        var tag = new TagBuilder(ElementTypes.DonationForm.TagName);
        
        tag.Attributes.Add("element-id", $"{ElementKind.DonationFormCampaign.ToEnumString()}_{Id}");
        tag.Attributes.Add("element-kind", $"{ElementKind.DonationFormCampaign.ToEnumString()}");

        return tag.ToHtmlString();
    }
    
    public string GetDonationButtonEmbedCode() {
        var tag = new TagBuilder(ElementTypes.DonationButton.TagName);
        
        tag.Attributes.Add("element-id", $"{ElementKind.DonationButtonCampaign.ToEnumString()}_{Id}");
        tag.Attributes.Add("element-kind", $"{ElementKind.DonationButtonCampaign.ToEnumString()}");

        return tag.ToHtmlString();
    }
}