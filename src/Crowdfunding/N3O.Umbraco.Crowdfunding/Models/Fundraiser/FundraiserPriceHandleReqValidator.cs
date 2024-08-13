using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserPriceHandleReqValidator : ModelValidator<FundraiserPriceHandleReq> {
    public FundraiserPriceHandleReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Amount)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAmount));
        
        RuleFor(x => x.Description)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyDescription));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify an amount";
        public string SpecifyDescription => "Please specify the description";
    }
}