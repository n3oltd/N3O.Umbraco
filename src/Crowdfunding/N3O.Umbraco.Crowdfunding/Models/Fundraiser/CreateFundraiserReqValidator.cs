using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserReqValidator : ModelValidator<CreateFundraiserReq> {
    private const int MaximumNameLength = 50;
    
    public CreateFundraiserReqValidator(IFormatter formatter,
                                        IContentLocator contentLocator,
                                        IProfanityGuard profanityGuard) 
        : base(formatter) {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyName));
        
        RuleFor(x => x.Name)
           .MaximumLength(MaximumNameLength)
           .When(x => x.Name.HasValue())
           .WithMessage(Get<Strings>(s => s.SpecifyName, MaximumNameLength));

        RuleFor(x => x.Name)
          .Must(x => !profanityGuard.HasAnyProfanity(x))
          .When(x => x.Name.HasValue())
          .WithMessage(Get<Strings>(s => s.UnacceptableName));
        
        RuleFor(x => x.Currency)
          .NotNull()
          .WithMessage(Get<Strings>(s => s.SpecifyCurrency));
        
        RuleFor(x => x.CampaignId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyCampaignId));
        
        RuleFor(x => x.CampaignId)
           .Must(x => CampaignIdIsValid(contentLocator, x.GetValueOrThrow()))
           .When(x => x.CampaignId.HasValue())
           .WithMessage(Get<Strings>(s => s.InvalidCampaign));
        
        RuleFor(x => x.Goals)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyGoals));
    }

    private bool CampaignIdIsValid(IContentLocator contentLocator, Guid campaignId) {
        return contentLocator.ById<CampaignContent>(campaignId).HasValue();
    }

    public class Strings : ValidationStrings {
        public string InvalidCampaign => "The specified campaign is invalid";
        public string SpecifyCampaignId => "Please specify the campaign ID";
        public string SpecifyCurrency => "Please specify the currency of the fundraiser";
        public string SpecifyGoals => "Please specify the goals for the fundraiser";
        public string SpecifyName => "Please specify the name of the fundraiser";
        public string UnacceptableName => "The name contains unacceptable characters or words";
    }
}
