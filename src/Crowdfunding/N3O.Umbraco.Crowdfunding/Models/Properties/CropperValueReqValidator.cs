using FluentValidation;
using N3O.Umbraco.CrowdFunding.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CropperValueReqValidator : ModelValidator<CropperValueReq> {
    public CropperValueReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.StorageToken)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));

        RuleFor(x => x.CropType)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyCropType));

        RuleFor(x => x.CircleCrop)
           .NotNull()
           .When(x => x.CropType == CropTypes.Circle)
           .WithMessage(Get<Strings>(x => x.SpecifyCropType));

        RuleFor(x => x.CircleCrop)
           .Null()
           .When(x => x.CropType != CropTypes.Circle)
           .WithMessage(Get<Strings>(x => x.CircleCropNotAllowed));

        RuleFor(x => x.RectangleCrop)
           .NotNull()
           .When(x => x.CropType == CropTypes.Rectangle)
           .WithMessage(Get<Strings>(x => x.SpecifyCropType));

        RuleFor(x => x.RectangleCrop)
           .Null()
           .When(x => x.CropType != CropTypes.Rectangle)
           .WithMessage(Get<Strings>(x => x.RectangleCropNotAllowed));
    }

    public class Strings : ValidationStrings {
        public string SpecifyValue => "Please specify the storage token";
        public string SpecifyCropType => "Please specify the crop type";
        public string CircleCropNotAllowed => "Circle crop cannot be specified for this crop type";
        public string RectangleCropNotAllowed => "Rectangle crop cannot be specified for this crop type";
    }
}