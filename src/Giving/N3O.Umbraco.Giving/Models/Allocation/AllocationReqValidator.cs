using FluentValidation;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class AllocationReqValidator : ModelValidator<AllocationReq> {
    public AllocationReqValidator(IFormatter formatter,
                                  ICurrencyAccessor currencyAccessor,
                                  IPricedAmountValidator pricedAmountValidator)
        : base(formatter) {
        var currentCurrency = currencyAccessor.GetCurrency();

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyType));

        RuleFor(x => x.FundDimensions)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyFundDimensions));
        
        RuleFor(x => x.Feedback)
           .Null()
           .When(x => x.Type != AllocationTypes.Feedback)
           .WithMessage(Get<Strings>(s => s.FeedbackAllocationNotAllowed));

        RuleFor(x => x.Fund)
            .NotNull()
            .When(x => x.Type == AllocationTypes.Fund)
            .WithMessage(Get<Strings>(s => s.SpecifyFundAllocation));

        RuleFor(x => x.Fund)
            .Null()
            .When(x => x.Type != AllocationTypes.Fund)
            .WithMessage(Get<Strings>(s => s.FundAllocationNotAllowed));

        RuleFor(x => x.Sponsorship)
            .NotNull()
            .When(x => x.Type == AllocationTypes.Sponsorship)
            .WithMessage(Get<Strings>(s => s.SpecifySponsorshipAllocation));

        RuleFor(x => x.Sponsorship)
            .Null()
            .When(x => x.Type != AllocationTypes.Sponsorship)
            .WithMessage(Get<Strings>(s => s.SponsorshipAllocationNotAllowed));
        
        RuleFor(x => x.Value)
            .Must((req, x) => pricedAmountValidator.IsValid(x, req.Fund.DonationItem, req.FundDimensions))
            .When(x => x.Value.HasValue() && x.Fund?.DonationItem?.HasPricing() == true)
            .WithMessage(Get<Strings>(s => s.InvalidValue));

        RuleForEach(x => x.Sponsorship.Components)
            .Must((req, x) => x.Component?.HasPricing() != true ||
                              pricedAmountValidator.IsValid(x.Value,
                                                            x.Component,
                                                            req.FundDimensions,
                                                            req.Sponsorship.Duration?.Months ?? 1))
            .When(x => x.Sponsorship.HasValue())
            .WithMessage(Get<Strings>(s => s.InvalidValue));

        RuleFor(x => x.Value)
            .Must((req, x) => x.Amount.GetValueOrThrow() == req.Sponsorship.Components.Sum(x => x.Value.Amount.GetValueOrThrow()))
            .When(x => x.Value.HasValue(v => v.Amount) && x.Sponsorship.HasValue() && x.Sponsorship.Components.All(c => c.Value.HasValue(v => v.Amount)))
            .WithMessage(Get<Strings>(s => s.InvalidValue));

        RuleFor(x => x.FundDimensions)
            .Must((req, x) => FundDimensionsAreValid(x, req.GetFundDimensionsOptions()))
            .When(x => x.FundDimensions.HasValue())
            .WithMessage(Get<Strings>(s => s.InvalidFundDimensions));

        ValidateCurrencies(currentCurrency);
    }

    private void ValidateCurrencies(Currency currency) {
        RuleFor(x => x.Value.Currency)
            .Must(x => x == currency)
            .When(x => x.HasValue(y => y.Value?.Currency))
            .WithMessage(Get<Strings>(s => s.CurrencyMismatch));

        RuleForEach(x => x.Sponsorship.Components)
            .Must(x => x.Value.Currency == currency)
            .When(x => x.Sponsorship.OrEmpty(y => y.Components).HasAny(c => c.Value.HasValue(v => v.Currency)))
            .WithMessage(Get<Strings>(s => s.CurrencyMismatch));
    }

    private bool FundDimensionsAreValid(FundDimensionValuesReq req, IFundDimensionsOptions options) {
        if (FundDimensionIsValid(req.Dimension1, options?.Dimension1Options) &&
            FundDimensionIsValid(req.Dimension2, options?.Dimension2Options) &&
            FundDimensionIsValid(req.Dimension3, options?.Dimension3Options) &&
            FundDimensionIsValid(req.Dimension4, options?.Dimension4Options)) {
            return true;
        }

        return false;
    }

    private bool FundDimensionIsValid<T>(T value, IEnumerable<T> options) where T : FundDimensionValue<T> {
        return value == null || options?.Contains(value) != false;
    }

    public class Strings : ValidationStrings {
        public string CurrencyMismatch => "All currencies must be the same and must match the currently active currency";
        public string FeedbackAllocationNotAllowed => "Feedback cannot be specified for this type of allocation";
        public string FundAllocationNotAllowed => "Fund cannot be specified for this type of allocation";
        public string InvalidFundDimensions => "One or more fund dimension values are invalid";
        public string InvalidValue => "Invalid value specified";
        public string SpecifyFundAllocation => "Please specify the fund allocation";
        public string SpecifyFundDimensions => "Please specify the fund dimensions";
        public string SpecifySponsorshipAllocation => "Please specify the sponsorship allocation";
        public string SponsorshipAllocationNotAllowed => "Sponsorship cannot be specified for this type of allocation";
        public string SpecifyType => "Please specify the allocation type";
    }
}
