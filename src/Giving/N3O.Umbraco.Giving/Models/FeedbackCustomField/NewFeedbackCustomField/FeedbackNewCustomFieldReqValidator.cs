using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldReqValidator : ModelValidator<FeedbackNewCustomFieldReq> {
    private const int AliasMaxLength = 100;
    
    public FeedbackNewCustomFieldReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Alias)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAlias));
        
        RuleFor(x => x.Alias)
           .MaximumLength(AliasMaxLength)
           .WithMessage(Get<Strings>(s => s.AliasTooLong_1, AliasMaxLength));
            
    }
    
    public class Strings : ValidationStrings {
        public string AliasTooLong_1 => "Alias exceeds maximum length of {0} characters";
        public string SpecifyAlias => "Please specify the custom field alias";
    }
}