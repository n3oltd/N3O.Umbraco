using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalReqValidator : ModelValidator<FundraiserGoalReq> {
    public FundraiserGoalReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Amount)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAmount));

        RuleFor(x => x.GoalId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyGoalId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify the amount";
        public string SpecifyGoalId => "Please specify the goal IDn";
    }
}