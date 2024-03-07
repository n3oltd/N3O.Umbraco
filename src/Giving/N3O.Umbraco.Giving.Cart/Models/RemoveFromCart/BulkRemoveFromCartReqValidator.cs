using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class BulkRemoveFromCartReqValidator : ModelValidator<BulkRemoveFromCartReq> {
    public BulkRemoveFromCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.GivingType)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyGivingType));
    
        RuleFor(x => x.Indexes)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyIndex));
    }

    public class Strings : ValidationStrings {
        public string SpecifyGivingType => "Please specify the giving type";
        public string SpecifyIndex => "Please specify the at least one index to bulk remove from cart";
    }
}
