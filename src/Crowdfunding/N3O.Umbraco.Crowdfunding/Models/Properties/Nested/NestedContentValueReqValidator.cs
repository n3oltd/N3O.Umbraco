using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedContentValueReqValidator : ModelValidator<NestedContentValueReq> {
    public NestedContentValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Items)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyItems));
    }

    public class Strings : ValidationStrings {
        public string SpecifyItems => "Please specify at least one item";
    }
}