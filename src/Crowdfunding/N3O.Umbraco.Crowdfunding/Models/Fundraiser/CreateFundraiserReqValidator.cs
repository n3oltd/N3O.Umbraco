using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserReqValidator : ModelValidator<CreateFundraiserReq> {
    private const int MaximumAllocations = 50;
    private const int MaximumTitleLength = 50;
    
    public CreateFundraiserReqValidator(IFormatter formatter,
                                        IContentLocator contentLocator,
                                        ICrowdfundingHelper crowdfundingHelper,
                                        IProfanityGuard profanityGuard) 
        : base(formatter) {
        RuleFor(x => x.Title)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyTitle));
        
        RuleFor(x => x.Title)
           .MaximumLength(MaximumTitleLength)
           .When(x => x.Title.HasValue())
           .WithMessage(Get<Strings>(s => s.SpecifyTitle, MaximumTitleLength));

        RuleFor(x => x.Title)
          .Must(x => !profanityGuard.ContainsProfanity(x))
          .When(x => x.Title.HasValue())
          .WithMessage(Get<Strings>(s => s.UnacceptableTitle));
           
        RuleFor(x => x.Title)
          .Must(crowdfundingHelper.IsFundraiserTitleAvailable)
          .When(x => x.Title.HasValue())
          .WithMessage(Get<Strings>(s => s.TitleUnavailable));
        
        RuleFor(x => x.AccountReference)
          .NotEmpty()
          .WithMessage(Get<Strings>(s => s.SpecifyAccountReference));
        
        RuleFor(x => x.DisplayName)
          .NotEmpty()
          .WithMessage(Get<Strings>(s => s.SpecifyName));
        
        RuleFor(x => x.CampaignId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyCampaignId));
        
        RuleFor(x => x.CampaignId)
           .Must(x => CampaignIdIsValid(contentLocator, x.GetValueOrThrow()))
           .When(x => x.CampaignId.HasValue())
           .WithMessage(Get<Strings>(s => s.InvalidCampaign));
        
        RuleFor(x => x.Goals)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyAllocations));
        
        RuleFor(x => x.Goals)
           .Must(x => x.OrEmpty().Count() <= MaximumAllocations)
           .WithMessage(Get<Strings>(s => s.TooManyAllocations));
    }

    private bool CampaignIdIsValid(IContentLocator contentLocator, Guid campaignId) {
        return contentLocator.ById<CampaignContent>(campaignId).HasValue();
    }

    public class Strings : ValidationStrings {
        public string InvalidCampaign => "The specified campaign is invalid";
        public string SpecifyAccountReference => "Please specify the account reference";
        public string SpecifyAllocations => "Please specify the allocations for the fundraiser";
        public string SpecifyCampaignId => "Please specify the campaign id";
        public string SpecifyName => "Please specify the name of the fundraiser";
        public string SpecifyTitle => "Please specify the title of the fundraiser";
        public string TitleUnavailable => "The title is not available";
        public string TooManyAllocations => $"A maximum of {MaximumAllocations} allocations are allowed";
        public string UnacceptableTitle => "The title contains unacceptable characters or words";
    }
}
