using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeebackCrowdfundingCartItemReqValidator : ModelValidator<FeebackCrowdfundingCartItemReq> {
    public FeebackCrowdfundingCartItemReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.CustomFields)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyCustomFields));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyCustomFields => "Custom fields must be specified";
    }
}