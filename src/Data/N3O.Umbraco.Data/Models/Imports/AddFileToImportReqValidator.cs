using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models;

public class AddFileToImportReqValidator : ModelValidator<AddFileToImportReq> {
    public AddFileToImportReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.File)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.FileMustBeSpecified));
    }
    
    public class Strings : ValidationStrings { 
        public string FileMustBeSpecified => "File must be specified";
    }
}
