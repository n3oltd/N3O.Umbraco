using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Data.Models {
    public class AddFileToImportReqValidator : ModelValidator<QueueImportsReq> {
        public AddFileToImportReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.ZipFile)
               .Must(x => x.Filename.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
               .When(x => x.ZipFile.HasValue())
               .WithMessage(Get<Strings>(s => s.InvalidZipFile));
        }
        
        public class Strings : ValidationStrings { 
            public string InvalidZipFile => "Invalid zip file";
        }
    }
}