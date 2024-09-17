using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsReqValidator : ModelValidator<FundraiserGoalsReq> {
    private const int MaximumGoals = 50;

    public FundraiserGoalsReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyGoals));

        RuleFor(x => x.Items)
            .Must(x => x.OrEmpty().Count() <= MaximumGoals)
            .WithMessage(Get<Strings>(s => s.TooManyGoals));
    }

    private bool CampaignIdIsValid(IContentLocator contentLocator, Guid campaignId) {
        return contentLocator.ById<CampaignContent>(campaignId).HasValue();
    }

    public class Strings : ValidationStrings {
        public string SpecifyGoals => "Please specify the goals for the fundraiser";
        public string TooManyGoals => $"A maximum of {MaximumGoals} goals are allowed";
    }
}