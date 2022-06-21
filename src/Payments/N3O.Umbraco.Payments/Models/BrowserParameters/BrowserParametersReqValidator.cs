using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Models;

public class BrowserParametersReqValidator : ModelValidator<BrowserParametersReq> {
    public BrowserParametersReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.ColourDepth)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyColourDepth));

        RuleFor(x => x.JavaEnabled)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyJavaEnabled));

        RuleFor(x => x.JavaScriptEnabled)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyJavaScriptEnabled));

        RuleFor(x => x.ScreenHeight)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyScreenHeight));

        RuleFor(x => x.ScreenWidth)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyScreenWidth));

        RuleFor(x => x.UtcOffsetMinutes)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyUtcOffsetMinutes));
    }

    public class Strings : ValidationStrings {
        public string SpecifyColourDepth => "Please specify colour depth";
        public string SpecifyJavaEnabled => "Please specify whether Java is enabled";
        public string SpecifyJavaScriptEnabled => "Please specify whether JavaScript is enabled";
        public string SpecifyScreenHeight => "Please specify screen height";
        public string SpecifyScreenWidth => "Please specify screen width";
        public string SpecifyUtcOffsetMinutes => "Please specify the UTC offset in minutes";
    }
}
