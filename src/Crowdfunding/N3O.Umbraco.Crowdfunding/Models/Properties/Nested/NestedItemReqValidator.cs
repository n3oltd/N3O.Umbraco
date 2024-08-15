using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedItemReqValidator : ModelValidator<NestedItemReq> {
    public NestedItemReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.ContentTypeAlias)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.ContentTypeAlias));
        
        RuleFor(x => x.Properties)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyProperties));
    }

    public class Strings : ValidationStrings {
        public string ContentTypeAlias => "Please specify the content type alias for the item";
        public string SpecifyProperties => "Please specify at least one property";
    }
}