using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models {
    public class QueueImportsReqValidator : ModelValidator<QueueImportsReq> {
        public QueueImportsReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.DatePattern)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyDatePattern));
            
            RuleFor(x => x.CsvFile)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyFile));

            RuleFor(x => x.CsvFile)
                .Must(x => x.ContentType.EqualsInvariant(DataConstants.ContentTypes.Csv))
                .WithMessage(Get<Strings>(s => s.InvalidFile));
        }

        public class Strings : ValidationStrings {
            public string InvalidFile => "File must be in CSV format";
            public string SpecifyDatePattern => "Date pattern must be specified";
            public string SpecifyFile => "File must be specified";
        }
    }
}