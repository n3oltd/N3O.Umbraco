using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    public CrowdfundingDataReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.CampaignId)
            .NotNull();
        
        // And so on
    }
}