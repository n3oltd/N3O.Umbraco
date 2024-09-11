using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDataReqValidator : ModelValidator<CrowdfunderDataReq> {
    private const int MaximumCommentLength = 200;
    
    private readonly IContentLocator _contentLocator;
    private readonly IProfanityGuard _profanityGuard;
    
    public CrowdfunderDataReqValidator(IFormatter formatter,
                                       IContentLocator contentLocator,
                                       IProfanityGuard profanityGuard) 
        : base(formatter) {
        _contentLocator = contentLocator;
        _profanityGuard = profanityGuard;
    }

    public override ValidationResult Validate(ValidationContext<CrowdfunderDataReq> context) {
        RuleFor(x => x.FundraiserId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyFundraiserId));
        
        RuleFor(x => x.FundraiserId)
           .Must(x => CanDonate(x.GetValueOrThrow()))
           .When(x => x.FundraiserId.HasValue)
           .WithMessage(Get<Strings>(s => s.FundraiserNotAcceptingDonations));
        
        RuleFor(x => x.Anonymous)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAnonymous));
        
        RuleFor(x => x.Comment)
           .MaximumLength(MaximumCommentLength)
           .When(x => x.Comment.HasValue())
           .WithMessage(Get<Strings>(s => s.CommentTooLong_1, MaximumCommentLength));
        
        RuleFor(x => x.Comment)
           .Must(x => !_profanityGuard.ContainsProfanity(x))
           .When(x => x.Comment.HasValue())
           .WithMessage(Get<Strings>(s => s.UnacceptableComment));

        return base.Validate(context);
    }

    private bool CanDonate(Guid fundraiserId) {
        var fundraiser = _contentLocator.ById<FundraiserContent>(fundraiserId);

        return fundraiser?.Status == FundraiserStatuses.Open;
    }

    public class Strings : ValidationStrings {
        public string CommentTooLong_1 => "Comment length exceeds the allowed maximum of {0} characters";
        public string FundraiserNotAcceptingDonations => "The specified fundraiser is not currently accepting donations";
        public string SpecifyAnonymous => "Please specify if the contribution is anonymous or not";
        public string SpecifyFundraiserId => "Please specify the fundraiser ID";
        public string UnacceptableComment => "The comment contains unacceptable characters or words";
    }
}