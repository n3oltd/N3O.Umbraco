using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models.AddToCart;

public class AddToCartReqValidator : ModelValidator<AddToCartReq> {
    public AddToCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Crowdfunding)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfunderData));
        
        RuleFor(x => x.Items)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyItems));
        
        RuleFor(x => x.Type)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyCrowdfunderType));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyCrowdfunderData => "Please specify the crowdfunder data";
        public string SpecifyCrowdfunderType => "Please specify the crowdfunder type";
        public string SpecifyItems => "Please select items to add to cart.";
    }
}