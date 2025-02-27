using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDataReqValidator : ModelValidator<CrowdfunderDataReq> {
    private const int MaximumCommentLength = 200;
    
    private readonly IProfanityGuard _profanityGuard;
    
    public CrowdfunderDataReqValidator(IFormatter formatter, IProfanityGuard profanityGuard) : base(formatter) {
        _profanityGuard = profanityGuard;
    }

    public override ValidationResult Validate(ValidationContext<CrowdfunderDataReq> context) {
        RuleFor(x => x.CrowdfunderId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfunderId));
        
        RuleFor(x => x.Anonymous)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAnonymous));
        
        RuleFor(x => x.Comment)
           .MaximumLength(MaximumCommentLength)
           .When(x => x.Comment.HasValue())
           .WithMessage(Get<Strings>(s => s.CommentTooLong_1, MaximumCommentLength));
        
        RuleFor(x => x.Comment)
           .Must(x => !_profanityGuard.HasAnyProfanity(x))
           .When(x => x.Comment.HasValue())
           .WithMessage(Get<Strings>(s => s.UnacceptableComment));

        return base.Validate(context);
    }

    public class Strings : ValidationStrings {
        public string CommentTooLong_1 => "Comment length exceeds the allowed maximum of {0} characters";
        public string SpecifyAnonymous => "Please specify if the contribution is anonymous or not";
        public string SpecifyCrowdfunderId => "Please specify the crowdfunder ID";
        public string UnacceptableComment => "The comment contains unacceptable characters or words";
    }
}