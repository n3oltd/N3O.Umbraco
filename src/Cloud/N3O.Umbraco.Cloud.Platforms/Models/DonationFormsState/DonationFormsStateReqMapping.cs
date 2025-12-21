using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    private readonly ILookups _lookups;
    private readonly ICdnClient _cdnClient;

    public ElementDonationFormsStateReqMapping(IContentLocator contentLocator, ILookups lookups, ICdnClient cdnClient) {
        _contentLocator = contentLocator;
        _lookups = lookups;
        _cdnClient = cdnClient;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, DonationFormStateReq>((_, _) => new DonationFormStateReq(), Map);
        mapper.Define<OfferingContent, DonationFormStateReq>((_, _) => new DonationFormStateReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OfferingContent src, DonationFormStateReq dest, MapperContext ctx) {
        var fixedFundDimensionValues = src.GetFixedFundDimensionValues();
        var campaign = src.Content().Parent.As<CampaignContent>();
        
        dest.CartItem = GetCartItemReq(campaign, src, fixedFundDimensionValues, null);
        dest.Options = GetDonationFormOptionsReq(ctx, src);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, DonationFormStateReq dest, MapperContext ctx) {
        var offering = src.GetOffering(_contentLocator);
        var campaign = offering.Content().Parent.As<CampaignContent>();
        var fixedFundDimensionValues = src.GetFixedFundDimensionValues(offering);
        
        dest.CartItem = GetCartItemReq(campaign, offering, fixedFundDimensionValues, src.Tags);
        dest.Options = ctx.Map<OfferingContent, DonationFormStateReq>(offering).Options;
    }
}