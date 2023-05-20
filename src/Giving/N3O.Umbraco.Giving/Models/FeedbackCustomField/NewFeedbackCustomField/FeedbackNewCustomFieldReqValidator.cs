using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldReqValidator : ModelValidator<FeedbackNewCustomFieldReq> {
    public FeedbackNewCustomFieldReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Alias)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAlias));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyAlias => "Please specify the custom field alias";
    }
}