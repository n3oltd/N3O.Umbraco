using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public class CompleteThreeDSecureReqValidator : ModelValidator<CompleteThreeDSecureReq> {
        public CompleteThreeDSecureReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.CRes)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.AllParametersRequired));
            
            RuleFor(x => x.ThreeDSessionData)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.AllParametersRequired));
        }

        public class Strings : ValidationStrings {
            public string AllParametersRequired => "All parameters are required";
        }
    }
}