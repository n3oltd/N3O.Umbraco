using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingAllocationExtension : AllocationExtension<CrowdfundingDataReq, CrowdfundingData> {
    private readonly IContentLocator _contentLocator;

    public CrowdfundingAllocationExtension(IJsonProvider jsonProvider,
                                           IValidator<CrowdfundingDataReq> validator,
                                           IContentLocator contentLocator)
        : base(jsonProvider, validator) {
        _contentLocator = contentLocator;
    }

    protected override CrowdfundingData Bind(CrowdfundingDataReq req) {
        var page = _contentLocator.ById<CrowdfundingPageContent>(req.PageId.GetValueOrThrow());
        
        return new CrowdfundingData(page.Campaign.Content().Key,
                                    page.Team?.Content().Key,
                                    page.Content().Key,
                                    page.Content().AbsoluteUrl(),
                                    req.Comment,
                                    req.Anonymous);
    }

    public override string Key => CrowdfundingConstants.Allocations.Extensions.Key;
}