using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models;

public class AddToCartReqValidator : ModelValidator<AddToCartReq> {
    public AddToCartReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.GivingType)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyGivingType));
    
        RuleFor(x => x.Allocation)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyAllocation));

        RuleFor(x => x.Quantity)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyQuantity));
        
        RuleFor(x => x.Allocation.Sponsorship)
            .NotNull()
            .When(x => x.GivingType == GivingTypes.Donation && x.Allocation.Type == AllocationTypes.Sponsorship)
            .WithMessage(Get<Strings>(s => s.SpecifySponsorshipDuration));
        
        RuleFor(x => x.Allocation.Sponsorship.Duration)
            .Null()
            .When(x => x.GivingType != GivingTypes.Donation &&
                       x.Allocation.HasValue(allocation => allocation.Sponsorship) && 
                       x.Allocation.Type == AllocationTypes.Sponsorship)
            .WithMessage(Get<Strings>(s => s.CannotSpecifySponsorshipDuration));
    }

    public class Strings : ValidationStrings {
        public string CannotSpecifySponsorshipDuration => "Sponsorship duration can only be specified for donations";
        public string SpecifyAllocation => "Please specify the allocation";
        public string SpecifyGivingType => "Please specify the giving type";
        public string SpecifyQuantity => "Please specify the quantity";
        public string SpecifySponsorshipDuration => "Please specify the sponsorship duration";
    }
}
