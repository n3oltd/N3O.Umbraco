using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldReqValidator : ModelValidator<FeedbackNewCustomFieldReq> {
    private readonly ILookups _lookups;

    public FeedbackNewCustomFieldReqValidator(IFormatter formatter, ILookups lookups) : base(formatter) {
        _lookups = lookups;
        
        RuleFor(x => x.Alias)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyAlias));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyAlias => "Please specify the custom field alias";
    }
}