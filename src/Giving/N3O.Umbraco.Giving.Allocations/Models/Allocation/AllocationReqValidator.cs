using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Giving.Pricing.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Model {
    public class AllocationReqValidator : ModelValidator<AllocationReq> {
        public AllocationReqValidator(IFormatter formatter, IPricing pricing) : base(formatter) {
            RuleFor(x => x.Type)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyType));
        
            RuleFor(x => x.Fund)
                .NotNull()
                .When(x => x.Type == AllocationTypes.Fund)
                .WithMessage(Get<Strings>(s => s.SpecifyDonationItem));

            RuleFor(x => x.Sponsorship)
                .NotNull()
                .When(x => x.Type == AllocationTypes.Sponsorship)
                .WithMessage(Get<Strings>(s => s.SpecifySponsorshipScheme));

            RuleFor(x => x.Dimension1)
                .Must((req, x) => req.Fund.DonationItem.Dimension1Options.Contains(x))
                .When(x => x.Fund.HasAny(x => x.DonationItem?.Dimension1Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension1, 1));

            RuleFor(x => x.Dimension2)
                .Must((req, x) => req.Fund.DonationItem.Dimension2Options.Contains(x))
                .When(x => x.Fund.HasAny(x => x.DonationItem?.Dimension2Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension2, 2));

            RuleFor(x => x.Dimension3)
                .Must((req, x) => req.Fund.DonationItem.Dimension3Options.Contains(x))
                .When(x => x.Fund.HasAny(x => x.DonationItem?.Dimension3Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension3, 3));

            RuleFor(x => x.Dimension4)
                .Must((req, x) => req.Fund.DonationItem.Dimension4Options.Contains(x))
                .When(x => x.Fund.HasAny(x => x.DonationItem?.Dimension4Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension4, 4));

            RuleFor(x => x.Dimension1)
                .Must((req, x) => req.Sponsorship.Scheme.Dimension1Options.Contains(x))
                .When(x => x.Sponsorship.HasAny(x => x.Scheme?.Dimension1Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension1, 1));

            RuleFor(x => x.Dimension2)
                .Must((req, x) => req.Sponsorship.Scheme.Dimension2Options.Contains(x))
                .When(x => x.Sponsorship.HasAny(x => x.Scheme?.Dimension2Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension2, 2));

            RuleFor(x => x.Dimension3)
                .Must((req, x) => req.Sponsorship.Scheme.Dimension3Options.Contains(x))
                .When(x => x.Sponsorship.HasAny(x => x.Scheme?.Dimension3Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension3, 3));

            RuleFor(x => x.Dimension4)
                .Must((req, x) => req.Sponsorship.Scheme.Dimension4Options.Contains(x))
                .When(x => x.Sponsorship.HasAny(x => x.Scheme?.Dimension4Options))
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2, x.Dimension4, 4));
        
        
            RuleFor(x => x.Value.Amount)
                .Must((req, x) => pricing.InCurrentCurrency(req.Fund.DonationItem).Amount == x)
                .When(x => x.HasValue(y => y.Value.Amount) && x.Fund?.DonationItem?.HasPrice() == true)
                .WithMessage(Get<Strings>(s => s.InvalidAmount));
        
            RuleFor(x => x.Value.Amount)
                .Must((req, x) => pricing.InCurrentCurrency(req.Sponsorship.Scheme).Amount == x)
                .When(x => x.HasValue(y => y.Value.Amount) && x.Sponsorship?.Scheme?.HasPrice() == true)
                .WithMessage(Get<Strings>(s => s.InvalidAmount));
        }

        public class Strings : ValidationStrings {
            public string InvalidAmount => "Invalid amount specified";
            public string InvalidDimensionValue_2 => $"{"{0}".Quote()} is not an allowed value for {1}";
            public string SpecifyDonationItem => "Please specify the donation item";
            public string SpecifySponsorshipScheme => "Please specify the sponsorship scheme";
            public string SpecifyType => "Please specify the allocation type";
        }
    }
}