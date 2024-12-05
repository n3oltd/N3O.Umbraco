using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipComponentAllocationReqValidator : ModelValidator<SponsorshipComponentAllocationReq> {
    public SponsorshipComponentAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Component)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyComponent));
        
        RuleFor(x => x.Value)
            .Must(x => x.HasValue())
            .WithMessage(Get<Strings>(s => s.SpecifyValue));
    }
}

public class Strings : ValidationStrings {
    public string SpecifyComponent => "Please specify the component";
    public string SpecifyValue => "Please specify the value";
}
