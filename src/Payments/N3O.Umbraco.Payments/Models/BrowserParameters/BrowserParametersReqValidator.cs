using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Models {
    public class BrowserParametersReqValidator : ModelValidator<BrowserParametersReq> {
        public BrowserParametersReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.ScreenWidth)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyScreenWidth));

            RuleFor(x => x.ScreenHeight)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyScreenHeight));
            
            RuleFor(x => x.ColorDepth)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyColor));
            
            RuleFor(x => x.JavaEnabled)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyJavaEnabled));
            
            RuleFor(x => x.JavaScriptEnabled)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyJavaScriptEnabled));
            
            RuleFor(x => x.Timezone)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.SpecifyTimezone));
        }

        public class Strings : ValidationStrings {
            public string SpecifyScreenWidth => "Please specify ScreenWidth";
            public string SpecifyScreenHeight => "Please specify ScreenHeight";
            public string SpecifyColor => "Please specify Color";
            public string SpecifyTimezone => "Please specify Timezone";
            public string SpecifyJavaScriptEnabled => "Please specify JavaScriptEnabled";
            public string SpecifyJavaEnabled => "Please specify JavaEnabled";
        }
    }
}