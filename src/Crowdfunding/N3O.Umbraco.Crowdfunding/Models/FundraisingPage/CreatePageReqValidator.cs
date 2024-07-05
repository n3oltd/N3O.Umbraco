using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreatePageReqValidator : ModelValidator<CreatePageReq> {
    public CreatePageReqValidator(IFormatter formatter) : base(formatter) { }
    private const int MaximumNameLength = 50;
    
    public CreatePageReqValidator(IFormatter formatter,
                                  IContentLocator contentLocator,
                                  IFundraisingPages fundraisingPages,
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
          .Must(x => !profanityGuard.ContainsProfanity(x))
          .When(x => x.Name.HasValue())
          .WithMessage(Get<Strings>(s => s.UnacceptableName));
           
        RuleFor(x => x.Name)
          .Must(fundraisingPages.IsPageNameAvailable)
          .When(x => x.Name.HasValue())
          .WithMessage(Get<Strings>(s => s.NameUnavailable));
        
        RuleFor(x => x.CampaignId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyCampaignId));
        
        RuleFor(x => x.CampaignId)
           .Must(x => CampaignIdIsValid(contentLocator, x.GetValueOrThrow()))
           .When(x => x.CampaignId.HasValue())
           .WithMessage(Get<Strings>(s => s.InvalidCampaign));
        
        RuleFor(x => x.Allocation)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyAllocation));
    }

    private bool CampaignIdIsValid(IContentLocator contentLocator, Guid campaignId) {
        return contentLocator.ById<CrowdfundingCampaignContent>(campaignId).HasValue();
    }

    public class Strings : ValidationStrings {
        public string InvalidCampaign => "The specified campaign is invalid";
        public string NameUnavailable => "The name is not available";
        public string SpecifyAllocation => "Please specify the allocation for the page";
        public string SpecifyCampaignId => "Please specify the campaign id";
        public string SpecifyName => "Please specify the name of the page";
        public string UnacceptableName => "The name contains unacceptable characters or words";
    }
}
