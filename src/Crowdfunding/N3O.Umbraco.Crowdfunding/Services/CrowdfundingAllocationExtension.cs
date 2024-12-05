using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingAllocationExtension : AllocationExtension<CrowdfunderDataReq, CrowdfunderData> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _urlBuilder;

    public CrowdfundingAllocationExtension(IJsonProvider jsonProvider,
                                           IValidator<CrowdfunderDataReq> validator,
                                           IContentLocator contentLocator,
                                           ICrowdfundingUrlBuilder urlBuilder)
        : base(jsonProvider, validator) {
        _contentLocator = contentLocator;
        _urlBuilder = urlBuilder;
    }

    protected override CrowdfunderData Bind(CrowdfunderDataReq req) {
        var crowdfunder = _contentLocator.ById(req.CrowdfunderId.GetValueOrThrow());
        var type = crowdfunder.ContentType.Alias.ToCrowdfunderType();

        ICrowdfunderContent crowdfunderContent;
        
        if (type == CrowdfunderTypes.Campaign) {
            crowdfunderContent = crowdfunder.As<CampaignContent>();
        } else if (type == CrowdfunderTypes.Fundraiser) {
            crowdfunderContent = crowdfunder.As<FundraiserContent>();
        } else {
            throw UnrecognisedValueException.For(type);
        }

        return new CrowdfunderData(crowdfunder.Key,
                                   crowdfunder.ContentType.Alias.ToCrowdfunderType(),
                                   crowdfunderContent.Name,
                                   crowdfunderContent.Url(_urlBuilder),
                                   req.Comment,
                                   req.Anonymous.GetValueOrThrow());
    }

    public override string Key => CrowdfundingConstants.Allocations.Extensions.Key;
}