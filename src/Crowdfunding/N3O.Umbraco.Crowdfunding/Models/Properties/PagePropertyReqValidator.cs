using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PagePropertyReqValidator : ModelValidator<PagePropertyReq> {
    // TODO Need to ensure based on type selected that the respective properties are all either null or not null
    // Need to ensure that the property with that "alias" is actually of "type" as this is a public API
    // We will probably need to implement an ICrowdfundingPagePropertyValidator that has an IsValidator(string alias)
    // so that in code we can define any custom validation rules around page properties and so that this code here
    // can remain generic and not need to change as we add property types.
    public PagePropertyReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Alias)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyAlias));
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyType));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyAlias => "Please specify the alias";
        public string SpecifyType => "Please specify the type";
    }
}