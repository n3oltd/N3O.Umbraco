using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class AddToCartReqValidator : ModelValidator<AddToCartReq> {
    public AddToCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.DonationType)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyDonationType));
        
        RuleFor(x => x.Allocation)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyAllocation));

        RuleFor(x => x.Quantity)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyQuantity));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAllocation => "Please specify the allocation";
        public string SpecifyDonationType => "Please specify the donation type";
        public string SpecifyQuantity => "Please specify the quantity";
    }
}
