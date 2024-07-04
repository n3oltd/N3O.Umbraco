using FluentValidation;
using Ganss.Xss;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class RawValueReqValidator : ModelValidator<RawValueReq> {
    public RawValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Value)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));
        
        RuleFor(x => x.Value)
           .Must(Sanitize)
           .WithMessage(Get<Strings>(x => x.ValueInvalid));
    }

    private bool Sanitize(string value) {
        try {
            var sanitizer = new HtmlSanitizer();

            sanitizer.Sanitize(value, null);

            return true;
        } catch {
            return false;
        }
    }

    public class Strings : ValidationStrings {
        public string SpecifyValue => "Please specify the value";
        public string ValueInvalid => "The value does not pass the validation rules";
    }
}