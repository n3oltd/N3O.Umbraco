using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
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
        var crowdfunder = _contentLocator.ById(req.CrowdfunderId.GetValueOrThrow());

        return new CrowdfunderData(crowdfunder.Key,
                                   crowdfunder.ContentType.Alias.ToCrowdfunderType(),
                                   req.Comment,
                                   req.Anonymous.GetValueOrThrow());
    }

    public override string Key => CrowdfundingConstants.Allocations.Extensions.Key;
}