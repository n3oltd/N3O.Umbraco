using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class BooleanValueReqValidator : ModelValidator<BooleanValueReq> {
    public BooleanValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Value)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyValue => "Please specify the value";
    }
}