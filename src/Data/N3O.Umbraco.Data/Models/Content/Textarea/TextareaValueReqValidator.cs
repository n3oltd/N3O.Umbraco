using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class TextareaValueReqValidator : ModelValidator<TextareaValueReq> {
    public TextareaValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Value)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));
    }

    public class Strings : ValidationStrings {
        public string SpecifyValue => "Please specify the value";
    }
}