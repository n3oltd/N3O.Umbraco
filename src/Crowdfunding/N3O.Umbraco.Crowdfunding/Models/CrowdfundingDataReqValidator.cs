using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

// TODO This is not validating the campaign ID, page ID and (if specified) team ID correspond to valid values
// It also doesn't validate the comment length or any banned words
public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    public CrowdfundingDataReqValidator(IFormatter formatter) : base(formatter) { }

    public override ValidationResult Validate(ValidationContext<CrowdfundingDataReq> context) {
        RuleFor(x => x.CampaignId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCampaignId));

        RuleFor(x => x.PageId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyPageId));
        
        RuleFor(x => x.Anonymous)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAnonymous));

        return base.Validate(context);
    }

    public class Strings : ValidationStrings {
        public string SpecifyAnonymous => "Please specify if the contribution is anonymous or not";
        public string SpecifyCampaignId => "Please specify the campaign ID";
        public string SpecifyPageId => "Please specify the page ID";
    }
}