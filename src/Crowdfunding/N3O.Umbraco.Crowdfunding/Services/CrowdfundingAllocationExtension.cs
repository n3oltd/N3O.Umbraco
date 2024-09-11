using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingAllocationExtension : AllocationExtension<CrowdfunderDataReq, CrowdfunderData> {
    private readonly IContentLocator _contentLocator;

    public CrowdfundingAllocationExtension(IJsonProvider jsonProvider,
                                           IValidator<CrowdfunderDataReq> validator,
                                           IContentLocator contentLocator)
        : base(jsonProvider, validator) {
        _contentLocator = contentLocator;
    }

    protected override CrowdfunderData Bind(CrowdfunderDataReq req) {
        var page = _contentLocator.ById<FundraiserContent>(req.FundraiserId.GetValueOrThrow());
        
        return new CrowdfunderData(page.Campaign.Content().Key,
                                   null,
                                   page.Content().Key,
                                   page.Content().AbsoluteUrl(),
                                   req.Comment,
                                   req.Anonymous);
    }

    public override string Key => CrowdfundingConstants.Allocations.Extensions.Key;
}