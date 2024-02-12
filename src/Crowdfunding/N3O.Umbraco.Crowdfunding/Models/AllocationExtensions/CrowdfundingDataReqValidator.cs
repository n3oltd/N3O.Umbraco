using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    public CrowdfundingDataReqValidator(IFormatter formatter) : base(formatter) { }

    public override ValidationResult Validate(ValidationContext<CrowdfundingDataReq> context) {
        RuleFor(x => x.CampaignId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyCampaignId));

        RuleFor(x => x.PageId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyPageId));

        RuleFor(x => x.PageUrl)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyPageUrl));

        /*RuleFor(x => x.TeamName)
           .Null()
           .When(x => !x.TeamId.HasValue)
          .WithMessage(Get<Strings>(s => s.TeamNameNotAllowed));

        RuleFor(x => x.TeamName)
           .NotNull()
           .When(x => x.TeamId.HasValue)
           .WithMessage(Get<Strings>(s => s.SpecifyTeamName));*/
        
        RuleFor(x => x.Anonymous)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyPresence));

        return base.Validate(context);
    }

    public class Strings : ValidationStrings {
        public string SpecifyCampaignId => "Please specify the campaign id";
        public string SpecifyPageId => "Please specify the page id";
        public string SpecifyPageUrl => "Please specify the page url";
        public string TeamNameNotAllowed => "Team name cannot be specified";
        public string SpecifyTeamName => "Please specify the team name";
        public string SpecifyPresence => "Please specify the presence";
    }
}