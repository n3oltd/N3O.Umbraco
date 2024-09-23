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
        
        RuleFor(x => x.GoalId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyGoalId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify the amount";
        public string SpecifyFundDimensions => "Please specify the fund dimensions";
        public string SpecifyGoalId => "Please specify the goal ID";
    }
}