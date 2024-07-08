using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PointReqValidator : ModelValidator<PointReq> {
    public PointReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.X)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyX));
        
        RuleFor(x => x.Y)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyY));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyX => "Please specify the X coordinate";
        public string SpecifyY => "Please specify the Y coordinate";
    }
}