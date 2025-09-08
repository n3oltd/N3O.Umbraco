using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Sync.Extensions.Models;

public class SyncDataReqValidator : ModelValidator<SyncDataReq> {
    public SyncDataReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Data)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.DataMustBeSpecified));
        
        RuleFor(x => x.SharedSecret)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SharedSecretMustBeSpecified));
    }

    public class Strings : ValidationStrings {
        public string DataMustBeSpecified => "Data must be specified";
        public string SharedSecretMustBeSpecified => "Shared secret must be specified";
    }
}