using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalPaymentReqValidator : ModelValidator<PayPalPaymentReq> {
        public PayPalPaymentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyEmail));

            RuleFor(x => x.TransactionId)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyTransactionId));
            
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(Get<Strings>(s => s.InvalidEmail));
        }

        public class Strings : ValidationStrings {
            public string InvalidEmail => "The specified email address is invalid";
            public string SpecifyEmail => "Please specify email";
            public string SpecifyTransactionId => "Please specify transaction ID";
        }
    }
}