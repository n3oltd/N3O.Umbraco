using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PageAllocationReqValidator : ModelValidator<PageAllocationReq> {
    public PageAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Title)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyTitle));
        
        RuleFor(x => x.Amount)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAmount));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify an amount";
        public string SpecifyTitle => "Please specify the allocation title";
    }
}