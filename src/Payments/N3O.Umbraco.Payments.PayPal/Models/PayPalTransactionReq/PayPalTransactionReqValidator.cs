using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalTransactionReqValidator : ModelValidator<PayPalTransactionReq> {
        public PayPalTransactionReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyEmail));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(Get<Strings>(s => s.InvalidEmail));
            
            RuleFor(x => x.AuthorizationId)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyTransactionId));
        }

        public class Strings : ValidationStrings {
            public string InvalidEmail => "The specified email address is invalid";
            public string SpecifyEmail => "Please specify email";
            public string SpecifyTransactionId => "Please specify authorization ID";
        }
    }
}