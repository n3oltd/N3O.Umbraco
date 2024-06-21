using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

// TODO
// (a) We need to use the PUBLISHED CONTENT to check the page ID exists (in a published state) and we
//     need to check the status of the page to make sure that it is still accepting donations.
//
// (b) We need to limit the comment to 200 characters and we also should inject a serive IProfanityDetector
//     based off a package like https://github.com/stephenhaunts/ProfanityDetector to fail if contains profanity.


public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    public CrowdfundingDataReqValidator(IFormatter formatter) : base(formatter) { }

    public override ValidationResult Validate(ValidationContext<CrowdfundingDataReq> context) {
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
        public string SpecifyPageId => "Please specify the page ID";
    }
}