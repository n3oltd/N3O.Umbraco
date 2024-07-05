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

public class CrowdfundingDataReqValidator : ModelValidator<CrowdfundingDataReq> {
    private const int MaximumCommentLength = 200;
    
    private readonly IContentLocator _contentLocator;
    private readonly IProfanityGuard _profanityGuard;
    
    public CrowdfundingDataReqValidator(IFormatter formatter,
                                        IContentLocator contentLocator,
                                        IProfanityGuard profanityGuard) 
        : base(formatter) {
        _contentLocator = contentLocator;
        _profanityGuard = profanityGuard;
    }

    public override ValidationResult Validate(ValidationContext<CrowdfundingDataReq> context) {
        RuleFor(x => x.PageId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyPageId));
        
        RuleFor(x => x.PageId)
           .Must(x => CanDonate(x.GetValueOrThrow()))
           .When(x => x.PageId.HasValue)
           .WithMessage(Get<Strings>(s => s.PageNotAcceptingDonations));
        
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

    private bool CanDonate(Guid pageId) {
        var page = _contentLocator.ById<CrowdfundingPageContent>(pageId);

        return page?.PageStatus == CrowdfundingPageStatuses.Open;
    }

    public class Strings : ValidationStrings {
        public string CommentTooLong_1 => "Comment length exceeds the allowed maximum of {0} characters";
        public string PageNotAcceptingDonations => "The specified page is not currently accepting donations";
        public string SpecifyAnonymous => "Please specify if the contribution is anonymous or not";
        public string SpecifyPageId => "Please specify the page ID";
        public string UnacceptableComment => "The comment contains unacceptable characters or words";
    }
}