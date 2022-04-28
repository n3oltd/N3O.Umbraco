using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Storage.Services;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Data.Models {
    public class QueueImportsReqValidator : ModelValidator<QueueImportsReq> {
        public QueueImportsReqValidator(IFormatter formatter, ITempStorage tempStorage) : base(formatter) {
            RuleFor(x => x.DatePattern)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyDatePattern));

            RuleFor(x => x.CsvFile)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyCsvFile));

            RuleFor(x => x.CsvFile)
                .Must(x => x.Filename.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
                .When(x => x.CsvFile.HasValue())
                .WithMessage(Get<Strings>(s => s.InvalidCsvFile));
            
            RuleFor(x => x.ZipFile)
                .Must(x => x.Filename.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
                .When(x => x.ZipFile.HasValue())
                .WithMessage(Get<Strings>(s => s.InvalidZipFile));
        }

        public class Strings : ValidationStrings {
            public string InvalidCsvFile => "Invalid CSV file";
            public string InvalidZipFile => "Invalid zip file";
            public string SpecifyCsvFile => "CSV file must be specified";
            public string SpecifyDatePattern => "Date pattern must be specified";
        }
    }
}