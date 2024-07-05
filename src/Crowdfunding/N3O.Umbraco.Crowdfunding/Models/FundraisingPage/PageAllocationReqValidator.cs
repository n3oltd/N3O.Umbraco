using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PageAllocationReqValidator : ModelValidator<PageAllocationReq> {
    public PageAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Title)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyTitle));
        
        RuleFor(x => x.Value)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyValue));
    }

    public class Strings : ValidationStrings {
        public string SpecifyTitle => "Please specify the allocation title";
        public string SpecifyValue => "Please specify the value";
    }
}