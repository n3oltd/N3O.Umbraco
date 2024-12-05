using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackAllocationReqValidator : ModelValidator<FeedbackAllocationReq> {
    public FeedbackAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Scheme)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyScheme));
    }

    public class Strings : ValidationStrings {
        public string SpecifyScheme => "Please specify the feedback scheme";
    }
}
