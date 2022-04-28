using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models {
    public class QueueImportsReqValidator : ModelValidator<QueueImportsReq> {
        public QueueImportsReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.DatePattern)
                .NotNull()
                .WithMessage("Date pattern must be specified");
            
            RuleFor(x => x.CsvFile)
                .NotNull()
                .WithMessage("File must be specified");
            
            RuleFor(x => x.CsvFile)
                .Must(x => x.ContentType.EqualsInvariant(DataConstants.ContentTypes.Csv))
                .WithMessage("File must be in CSV format");
        }
    }
}