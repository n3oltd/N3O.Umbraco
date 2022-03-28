using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public class StoreCardReqValidator : ModelValidator<StoreCardReq> {
        public StoreCardReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyToken));
            
            RuleFor(x => x.BrowserParameters)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyBrowserParameters));

            RuleFor(x => x.ReturnUrl)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyReturnUrl));
        }

        public class Strings : ValidationStrings {
            public string SpecifyBrowserParameters => "Please specify the browser parameters";
            public string SpecifyReturnUrl => "Please specify the return URL";
            public string SpecifyToken => "Please specify the token";
        }
    }
}