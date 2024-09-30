using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models.AddToCart;

public class AddToCartItemReqValidator : ModelValidator<AddToCartItemReq> {
    public AddToCartItemReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Value)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));
        
        RuleFor(x => x.Value)
           .NotEmpty()
           .When(x => x.Value.Amount < 0.1m)
           .WithMessage(Get<Strings>(x => x.SpecifyValueAmount));
        
        RuleFor(x => x.GoalId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyGoalId));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyGoalId => "Please specify the goal ID";
        public string SpecifyValue => "Please specify the value";
        public string SpecifyValueAmount => "Please specify the value amount";
    }
}