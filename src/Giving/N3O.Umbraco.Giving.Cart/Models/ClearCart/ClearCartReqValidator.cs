using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class ClearCartReqValidator : ModelValidator<ClearCartReq> {
    public ClearCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.GivingType)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyGivingType));
    }

    public class Strings : ValidationStrings {
        public string SpecifyGivingType => "Please specify the giving type";
    }
}
