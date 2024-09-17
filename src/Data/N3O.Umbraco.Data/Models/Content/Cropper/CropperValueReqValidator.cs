using FluentValidation;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class CropperValueReqValidator : ModelValidator<CropperValueReq> {
    public CropperValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.StorageToken)
            .NotNull()
            .WithMessage(Get<Strings>(x => x.SpecifyStorageToken));

        RuleFor(x => x.Shape)
            .NotNull()
            .WithMessage(Get<Strings>(x => x.SpecifyCropShape));

        RuleFor(x => x.Circle)
            .NotNull()
            .When(x => x.Shape == CropShapes.Circle)
            .WithMessage(Get<Strings>(x => x.SpecifyCircle));

        RuleFor(x => x.Circle)
            .Null()
            .When(x => x.Shape != CropShapes.Circle)
            .WithMessage(Get<Strings>(x => x.CircleCropNotAllowed));

        RuleFor(x => x.Rectangle)
            .NotNull()
            .When(x => x.Shape == CropShapes.Rectangle)
            .WithMessage(Get<Strings>(x => x.SpecifyRectangle));

        RuleFor(x => x.Rectangle)
            .Null()
            .When(x => x.Shape != CropShapes.Rectangle)
            .WithMessage(Get<Strings>(x => x.RectangleCropNotAllowed));
    }

    public class Strings : ValidationStrings {
        public string CircleCropNotAllowed => "Circle crop cannot be specified";
        public string RectangleCropNotAllowed => "Rectangle crop cannot be specified";
        public string SpecifyCircle => "Please specify the circle crop";
        public string SpecifyCropShape => "Please specify the crop shape";
        public string SpecifyRectangle => "Please specify the rectangle crop";
        public string SpecifyStorageToken => "Please specify the storage token";
    }
}