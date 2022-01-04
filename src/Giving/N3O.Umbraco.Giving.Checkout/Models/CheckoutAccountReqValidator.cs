using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutAccountReqValidator : ModelValidator<CheckoutAccountReq> {
        public CheckoutAccountReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Account)
                .NotNull()
                .WithMessage(Get<Strings>(x => x.SpecifyAccount));

            RuleFor(x => x.AcceptTerms)
                .Must(x => x)
                .WithMessage(Get<Strings>(x => x.AcceptTermsConditions));
        }

        public class Strings : ValidationStrings {
            public string AcceptTermsConditions => "To continue you must read and accept our terms and conditions";
            public string SpecifyAccount => "Please specify account details";
        }
    }
}