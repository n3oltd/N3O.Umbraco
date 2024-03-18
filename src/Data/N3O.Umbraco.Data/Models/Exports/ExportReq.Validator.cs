using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class ExportReqValidator : ModelValidator<ExportReq> {
    public ExportReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.IncludeUnpublished)
            .NotNull()
            .WithMessage("Please specify whether to include unpublished records or not");
    }
}
