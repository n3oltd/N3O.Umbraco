using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class RemoveFromCartReqValidator : ModelValidator<RemoveFromCartReq> {
    public RemoveFromCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.GivingType)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyGivingType));
    
        RuleFor(x => x.Index)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyIndex));
    }

    public class Strings : ValidationStrings {
        public string SpecifyGivingType => "Please specify the giving type";
        public string SpecifyIndex => "Please specify the index";
    }
}
