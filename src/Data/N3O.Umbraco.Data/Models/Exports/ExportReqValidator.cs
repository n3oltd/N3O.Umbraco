using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class ExportReqValidator : ModelValidator<ExportReq> {
    public ExportReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Format)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyFormat));
        
        RuleFor(x => x.IncludeUnpublished)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyIncludeUnpublished));

        RuleFor(x => x)
            .Must(x => x.Properties.HasAny() || x.Metadata.HasAny())
            .WithMessage(Get<Strings>(s => s.SpecifyPropertyOrMetadata));
    }

    public class Strings : ValidationStrings {
        public string SpecifyFormat => "Please specify the export format";
        public string SpecifyIncludeUnpublished => "Please specify whether to include unpublished records or not";
        public string SpecifyPropertyOrMetadata => "At least one property or metadata field must be specified";
    }
}
