using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceHandleReqValidator : ModelValidator<PriceHandleReq> {
    public PriceHandleReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyAmount));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify an amount";
    }
}
