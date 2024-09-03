﻿using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserAllocationReqValidator : ModelValidator<FundraiserAllocationReq> {
    public FundraiserAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Amount)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAmount));

        RuleFor(x => x.GoalId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyGoalId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAmount => "Please specify the amount for the allocation";
        public string SpecifyGoalId => "Please specify the goal id for the allocation";
    }
}