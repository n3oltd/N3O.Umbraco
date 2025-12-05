using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public ElementDonationFormsStateReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, DonationFormStateReq>((_, _) => new DonationFormStateReq(), Map);
        mapper.Define<OfferingContent, DonationFormStateReq>((_, _) => new DonationFormStateReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OfferingContent src, DonationFormStateReq dest, MapperContext ctx) {
        var oneTimeSuggestedAmounts = src.Fund?.OneTimeSuggestedAmounts;
        var recurringSuggestedAmounts = src.Fund?.RecurringSuggestedAmounts;
        var fixedFundDimensionValues = src.GetFixedFundDimensionValues();
        var campaign = src.Content().Parent.As<CampaignContent>();
        
        dest.Options = GetDonationFormOptionsReq(ctx,
                                                 fixedFundDimensionValues,
                                                 (GiftTypes.OneTime, oneTimeSuggestedAmounts),
                                                 (GiftTypes.OneTime, recurringSuggestedAmounts));
        dest.CartItem = GetCartItemReq(campaign, src, fixedFundDimensionValues, null);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, DonationFormStateReq dest, MapperContext ctx) {
        var offering = src.GetOffering(_contentLocator);
        var campaign = offering.Content().Parent.As<CampaignContent>();
        var fixedFundDimensionValues = src.GetFixedFundDimensionValues(offering);
        
        dest.Options = GetDonationFormOptionsReq(ctx, fixedFundDimensionValues);
        dest.CartItem = GetCartItemReq(campaign, offering, fixedFundDimensionValues, src.Tags);
    }
}