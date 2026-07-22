using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using AllocationType = N3O.Umbraco.Giving.Allocations.Lookups.AllocationType;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Offerings.CompositionAlias)]
public class OfferingContent :
    UmbracoContent<OfferingContent>, IHoldDonationFormStateContent, IHoldDonationFormContentContent {
    public CampaignContent Campaign => Content().Parent().As<CampaignContent>();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);

        FormContent = new DonationFormContentContent();
        FormContent.SetContent(content);

        FormState = new DonationFormStateContent();
        FormState.SetContent(content);
    }

    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);

        FormContent?.SetVariationContext(variationContext);
        FormState?.SetVariationContext(variationContext);
    }

    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public string Notes => GetValue(x => x.Notes);

    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);
    public string DonationPopupEmbedCode => GetValue(x => x.DonationPopupEmbedCode);

    public DonationFormContentContent FormContent { get; private set; }
    public DonationFormStateContent FormState { get; private set; }
    public bool AllowCrowdfunding => GetValue(x => x.AllowCrowdfunding);

    public void PopulateContributionInfo(ICdnClient cdnClient, PlatformsContributionInfoReq platformsContribution) {
        Campaign.PopulateContributionInfo(cdnClient, platformsContribution);

        platformsContribution.Offering = new OfferingInfoReq();
        platformsContribution.Offering.Id = Key.ToString();
        platformsContribution.Offering.Name = Name;
    }

    public void PopulateOptions(DonationFormOptionsReq options) {
        if (Campaign.Type == CampaignTypes.Qurbani) {
            options.Qurbani = new DonationFormQurbaniOptionsReq();

            options.Qurbani.Categories = FormState.Qurbani
                                                  .Categories
                                                  .OrEmpty()
                                                  .Select(c => c.Content().Key.ToString())
                                                  .ToList();
        }
    }

    public AllocationType Type => FormState.Type;
    public bool HasPricing => FormState.HasPricing;
}
