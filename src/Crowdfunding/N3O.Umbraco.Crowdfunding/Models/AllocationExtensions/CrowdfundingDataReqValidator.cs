using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    public CrowdfundingDataReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.CampaignId)
           .NotNull();
        
        RuleFor(x => x.PageId)
           .NotNull();

        // And so on
    }
}