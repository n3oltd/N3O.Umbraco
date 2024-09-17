using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class CircleCropReqValidator : ModelValidator<CircleCropReq> {
    public CircleCropReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Center)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyCenter));
        
        RuleFor(x => x.Radius)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyRadius));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyCenter => "Please specify the center point";
        public string SpecifyRadius => "Please specify the radius";
    }
}