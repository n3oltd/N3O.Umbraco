using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Offering : ContentOrPublishedLookup {
    public Offering(string id, string name, Guid? contentId, string campaignId) : base(id, name, contentId) {
        CampaignId = campaignId;
    }
    
    public string CampaignId { get; }
    
    public string GetDonationFormEmbedCode() {
        var tag = new TagBuilder(ElementTypes.DonationForm.TagName);
        
        tag.Attributes.Add("element-id", $"{Id}");
        tag.Attributes.Add("element-kind", $"{ElementKind.DonationFormOffering.ToEnumString()}");

        return tag.ToHtmlString();
    }
    
    public string GetDonationButtonEmbedCode() {
        var tag = new TagBuilder(ElementTypes.DonationButton.TagName);
        
        tag.Attributes.Add("element-id", $"{Id}");
        tag.Attributes.Add("element-kind", $"{ElementKind.DonationButtonOffering.ToEnumString()}");

        return tag.ToHtmlString();
    }
}