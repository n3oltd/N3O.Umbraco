using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class UpdateFundraiserGoalsReqValidator : ModelValidator<UpdateFundraiserGoalsReq> {
    private const int MaximumAllocations = 50;
    
    public UpdateFundraiserGoalsReqValidator(IFormatter formatter) : base(formatter) {
        
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
        public string SpecifyAllocations => "Please specify the allocations for the fundraiser";
        public string TooManyAllocations => $"A maximum of {MaximumAllocations} allocations are allowed";
    }
}
