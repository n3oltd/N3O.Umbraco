using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentAllocationReqValidator : ModelValidator<FeedbackComponentAllocationReq> {
    public FeedbackComponentAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Component)
            .NotNull()
            .WithMessage(Get<Stringss>(s => s.SpecifyComponent));
        
        RuleFor(x => x.Value)
            .Must(x => x.HasValue())
            .WithMessage(Get<Stringss>(s => s.SpecifyValue));
    }
}

public class Stringss : ValidationStrings {
    public string SpecifyComponent => "Please specify the component";
    public string SpecifyValue => "Please specify the value";
}
