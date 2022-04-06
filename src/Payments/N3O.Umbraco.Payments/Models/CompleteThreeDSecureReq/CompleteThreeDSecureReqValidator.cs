using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Models {
    public class CompleteThreeDSecureReqValidator : ModelValidator<CompleteThreeDSecureReq> {
        public CompleteThreeDSecureReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x)
                .Must(x => x.CRes.HasValue() || x.PaRes.HasValue())
                .WithMessage(Get<Strings>(s => s.SpecifyExactlyOneOfCResOrPaRes));
            
            RuleFor(x => x)
                .Must(x => !x.CRes.HasValue() || !x.PaRes.HasValue())
                .WithMessage(Get<Strings>(s => s.SpecifyExactlyOneOfCResOrPaRes));
        }
    }

    public class Strings : ValidationStrings {
        public string SpecifyExactlyOneOfCResOrPaRes => "Please specify exactly one of CRes or PaRes";
    }
}