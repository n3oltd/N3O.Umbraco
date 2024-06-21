using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Json;
using Umbraco.Extensions;

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
        var page = _contentLocator.ById(req.PageId.GetValueOrThrow());
        
        return new CrowdfundingData(req.CampaignId.GetValueOrThrow(),
                                    req.TeamId,
                                    req.PageId.GetValueOrThrow(),
                                    page.Url(),
                                    req.Comment,
                                    req.Anonymous);
    }

    public override string Key => CrowdfundingConstants.Allocations.Extensions.Key;
}