using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Read.Core.Models;

namespace N3O.Umbraco.Giving.Cart.Models;

public class AddUpsellToCartReqValidator : ModelValidator<AddUpsellToCartReq> {
    public AddUpsellToCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.UpsellItemId)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.IdMustBeSpecified));
    }

    public class Strings : ValidationStrings {
        public string IdMustBeSpecified => "Upsell Item Id must be specified";
    }
}
