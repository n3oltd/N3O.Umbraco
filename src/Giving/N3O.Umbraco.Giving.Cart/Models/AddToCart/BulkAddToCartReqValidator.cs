using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class BulkAddToCartReqValidator : ModelValidator<BulkAddToCartReq> {
    public BulkAddToCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyItems));
    }

    public class Strings : ValidationStrings {
        public string SpecifyItems => "Items must be specified";
    }
}