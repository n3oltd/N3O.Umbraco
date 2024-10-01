using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingCartReqValidator : ModelValidator<CrowdfundingCartReq> {
    public CrowdfundingCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Crowdfunding)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfundingData));
        
        RuleFor(x => x.Items)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyItems));
        
        RuleFor(x => x.Type)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfunderType));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyCrowdfundingData => "Please specify the crowdfunding data";
        public string SpecifyCrowdfunderType => "Please specify the crowdfunder type";
        public string SpecifyItems => "Please specify the items";
    }
}