using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class RectangleCropReqValidator : ModelValidator<RectangleCropReq> {
    public RectangleCropReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.BottomLeft)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyBottomLeft));
        
        RuleFor(x => x.TopRight)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyTopRight));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyBottomLeft => "Please specify the bottom left point";
        public string SpecifyTopRight => "Please specify the top right point";
    }
}