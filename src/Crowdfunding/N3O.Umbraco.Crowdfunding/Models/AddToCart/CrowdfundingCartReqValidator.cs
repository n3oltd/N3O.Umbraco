using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingCartReqValidator : ModelValidator<CrowdfundingCartReq> {
    public CrowdfundingCartReqValidator(IFormatter formatter, IContentLocator contentLocator) : base(formatter) {
        RuleFor(x => x.Crowdfunding)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfundingData));
        
        RuleFor(x => x.Items)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyItems));
        
        RuleFor(x => x.Type)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfunderType));
        
        RuleFor(x => x)
           .Must(x => IsCrowdfunderActive(contentLocator, x))
           .WithMessage(Get<Strings>(s => s.CrowdfunderInactive));
    }

    private bool IsCrowdfunderActive(IContentLocator contentLocator, CrowdfundingCartReq req) {
        var crowdfudner = contentLocator.GetCrowdfunderContent(req.Crowdfunding.CrowdfunderId.GetValueOrThrow(), req.Type);

        return crowdfudner?.Status == CrowdfunderStatuses.Active;
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyCrowdfundingData => "Please specify the crowdfunding data";
        public string SpecifyCrowdfunderType => "Please specify the crowdfunder type";
        public string SpecifyItems => "Please specify the items";
        public string CrowdfunderInactive => "Cannot donate towards an inactive page";
    }
}