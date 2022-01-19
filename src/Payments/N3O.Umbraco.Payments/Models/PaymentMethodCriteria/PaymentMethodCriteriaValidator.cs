using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Criteria {
    public class PaymentMethodCriteriaValidator : ModelValidator<PaymentMethodCriteria> {
        public PaymentMethodCriteriaValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Country)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyCountry));

            RuleFor(x => x.Currency)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyCurrency));
        }

        public class Strings : ValidationStrings {
            public string SpecifyCountry => "Please specify country";
            public string SpecifyCurrency => "Please specify currency";
        }
    }
}