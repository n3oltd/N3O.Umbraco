using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using ECommerceStage = N3O.Umbraco.Cloud.Platforms.Lookups.ECommerceStage;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.CrossSells.CompositionAlias)]
public class CrossSellContent :
    UmbracoContent<CrossSellContent>, IHoldDonationFormStateContent, IHoldDonationFormContentContent {
    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public ECommerceStage Stage => GetValue(x => x.Stage);
    public IEnumerable<Campaign> TargetCampaigns => GetValue(x => x.TargetCampaigns);

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

    public DonationFormContentContent FormContent { get; private set; }
    public DonationFormStateContent FormState { get; private set; }

    public void PopulateContributionInfo(ICdnClient _, PlatformsContributionInfoReq platformsContribution) {
        platformsContribution.CrossSell = new CrossSellInfoReq();
        platformsContribution.CrossSell.Id = Key.ToString();
        platformsContribution.CrossSell.Name = Name;
    }

    public void PopulateOptions(DonationFormOptionsReq options) { }
}
