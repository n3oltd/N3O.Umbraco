using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class ThreeDSecureChallengeReqValidator : ModelValidator<ThreeDSecureChallengeReq> {
        public ThreeDSecureChallengeReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.CRes)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyCRes));
            
            RuleFor(x => x.ThreeDsSessionData)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyThreeDsSessionData));
        }
    }

    public class Strings : ValidationStrings {
        public string SpecifyCRes => "Please specify the CRes value";
        public string SpecifyThreeDsSessionData => "Please specify the ThreeDsSessionData value";
    }
}