using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public class ChargeCardReqValidator : ModelValidator<ChargeCardReq> {
        public ChargeCardReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyToken));

            // TODO Add validation for (configurable) minimum payment value
            RuleFor(x => x.Value)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyValue));

            RuleFor(x => x.BrowserParameters)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyBrowserParameters));
            
            // RuleFor(x => x.ChallengeWindowSize)
            //     .NotEmpty()
            //     .WithMessage(Get<Strings>(x => x.SpecifyChallengeWindowSize));
            //
            RuleFor(x => x.ReturnUrl)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyReturnUrl));
        }

        public class Strings : ValidationStrings {
            public string SpecifyBrowserParameters => "Please specify the browser parameters";
            public string SpecifyChallengeWindowSize => "Please specify the challenge window size";
            public string SpecifyToken => "Please specify the token";
            public string SpecifyReturnUrl => "Please specify the return URL";
            public string SpecifyValue => "Please specify the value";
        }
    }
}