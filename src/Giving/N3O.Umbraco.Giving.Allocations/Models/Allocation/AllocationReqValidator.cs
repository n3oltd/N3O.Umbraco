using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Giving.Pricing.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace N3O.Umbraco.Giving.Allocations.Model {
    public class AllocationReqValidator : ModelValidator<AllocationReq> {
        public AllocationReqValidator(IFormatter formatter,
                                      IPricing pricing,
                                      IFundStructureAccessor fundStructureAccessor)
            : base(formatter) {
            var fundStructure = fundStructureAccessor.GetFundStructure();
            
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

            RuleFor(x => x.Value.Amount)
                .Must((req, x) => pricing.InCurrentCurrency(req.Fund.DonationItem).Amount == x)
                .When(x => x.HasValue(y => y.Value.Amount) && x.Fund?.DonationItem?.HasPrice() == true)
                .WithMessage(Get<Strings>(s => s.InvalidAmount));
        
            RuleFor(x => x.Value.Amount)
                .Must((req, x) => pricing.InCurrentCurrency(req.Sponsorship.Scheme).Amount == x)
                .When(x => x.HasValue(y => y.Value.Amount) && x.Sponsorship?.Scheme?.HasPrice() == true)
                .WithMessage(Get<Strings>(s => s.InvalidAmount));
            
            ValidateDimensions(fundStructure);
        }

        private void ValidateDimensions(FundStructure fundStructure) {
            ValidateFundDimensions(fundStructure);
            ValidateSponsorshipDimensions(fundStructure);
        }
        
        private void ValidateFundDimensions(FundStructure fundStructure) {
            ValidateDimension(x => x.Dimension1,
                              x => x.Fund?.DonationItem,
                              x => x.Dimension1Options,
                              fundStructure.Dimension1);

            ValidateDimension(x => x.Dimension2,
                              x => x.Fund?.DonationItem,
                              x => x.Dimension2Options,
                              fundStructure.Dimension2);

            
            ValidateDimension(x => x.Dimension3,
                              x => x.Fund?.DonationItem,
                              x => x.Dimension3Options,
                              fundStructure.Dimension3);

            
            ValidateDimension(x => x.Dimension4,
                              x => x.Fund?.DonationItem,
                              x => x.Dimension4Options,
                              fundStructure.Dimension4);
        }
        
        private void ValidateSponsorshipDimensions(FundStructure fundStructure) {
            ValidateDimension(x => x.Dimension1,
                              x => x.Sponsorship?.Scheme,
                              x => x.Dimension1Options,
                              fundStructure.Dimension1);

            ValidateDimension(x => x.Dimension2,
                              x => x.Sponsorship?.Scheme,
                              x => x.Dimension2Options,
                              fundStructure.Dimension2);

            
            ValidateDimension(x => x.Dimension3,
                              x => x.Sponsorship?.Scheme,
                              x => x.Dimension3Options,
                              fundStructure.Dimension3);

            
            ValidateDimension(x => x.Dimension4,
                              x => x.Sponsorship?.Scheme,
                              x => x.Dimension4Options,
                              fundStructure.Dimension4);
        }

        private void ValidateDimension<T, TOption>(Expression<Func<AllocationReq, TOption>> expression,
                                                   Func<AllocationReq, IHoldFundDimensionOptions> getHoldFundDimensionOptions,
                                                   Func<IHoldFundDimensionOptions, IEnumerable<TOption>> getOptions,
                                                   FundDimension<T, TOption> fundDimension)
            where T : FundDimension<T, TOption>
            where TOption : FundDimensionOption<TOption> {
            RuleFor(expression)
                .NotNull()
                .When(_ => fundDimension.IsActive)
                .WithMessage(_ => Get<Strings>(s => s.SpecifyDimensionValue_1,
                                               fundDimension.Name));
            
            RuleFor(expression)
                .Must((req, x) => getOptions( getHoldFundDimensionOptions(req)).Contains(x))
                .When(x => fundDimension.IsActive &&
                           expression.Compile().Invoke(x).HasValue() &&
                           getHoldFundDimensionOptions(x).HasValue() &&
                           getOptions(getHoldFundDimensionOptions(x)).HasAny())
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2,
                                               expression.Compile().Invoke(x).Name,
                                               fundDimension.Name));
        }

        public class Strings : ValidationStrings {
            public string InvalidAmount => "Invalid amount specified";
            public string InvalidDimensionValue_2 => $"{"{0}".Quote()} is not an allowed value for {"{1}".Quote()}";
            public string SpecifyDimensionValue_1 => $"Please specify a value for {"{0}".Quote()}";
            public string SpecifyDonationItem => "Please specify the donation item";
            public string SpecifySponsorshipScheme => "Please specify the sponsorship scheme";
            public string SpecifyType => "Please specify the allocation type";
        }
    }
}