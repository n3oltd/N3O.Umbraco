using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class ImportTemplateReqValidator : ModelValidator<ImportTemplateReq> {
    public ImportTemplateReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Properties)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyProperty));
    }

    public class Strings : ValidationStrings {
        public string SpecifyProperty => "At least one property must be specified";
    }
}
