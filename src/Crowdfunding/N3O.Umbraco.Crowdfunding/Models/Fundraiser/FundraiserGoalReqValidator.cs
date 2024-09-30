using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalReqValidator : ModelValidator<FundraiserGoalReq> {
    public FundraiserGoalReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Amount)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAmount));

        RuleFor(x => x.FundDimensions)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyFundDimensions));
        
        RuleFor(x => x.GoalOptionId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyGoalOptionId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify the amount";
        public string SpecifyFundDimensions => "Please specify the fund dimensions";
        public string SpecifyGoalOptionId => "Please specify the goal option ID";
    }
}