using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Donations.Content;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.ViewModels {
    public class SponsorshipDonationFormViewModel {
        private readonly IFormatter _formatter;

        public SponsorshipDonationFormViewModel(ICurrencyAccessor currencyAccessor,
                                                IFormatter formatter,
                                                IPricing pricing,
                                                IUmbracoMapper mapper,
                                                SponsorshipDonationOption sponsorshipOption,
                                                DonationType type) {
            _formatter = formatter;
        
            Scheme = sponsorshipOption.Scheme;
            Type = type;
            Currency = currencyAccessor.GetCurrency();
            FundDimension1 = GetFundDimension(Scheme.Dimension1Options, Scheme.DefaultFundDimension1());
            FundDimension2 = GetFundDimension(Scheme.Dimension2Options, Scheme.DefaultFundDimension2());
            FundDimension3 = GetFundDimension(Scheme.Dimension3Options, Scheme.DefaultFundDimension3());
            FundDimension4 = GetFundDimension(Scheme.Dimension4Options, Scheme.DefaultFundDimension4());
            Price = mapper.Map<Money, MoneyRes>(Scheme.GetPrice(pricing, Currency, type));
            QuantityOptions = GetQuantityOptions();
        }

        public SponsorshipScheme Scheme { get; }
        public DonationType Type { get; }
        public Currency Currency { get; }
        public FixedOrDefaultFundDimensionOption<FundDimension1Option> FundDimension1 { get; }
        public FixedOrDefaultFundDimensionOption<FundDimension2Option> FundDimension2 { get; }
        public FixedOrDefaultFundDimensionOption<FundDimension3Option> FundDimension3 { get; }
        public FixedOrDefaultFundDimensionOption<FundDimension4Option> FundDimension4 { get; }
        public MoneyRes Price { get; }
        public IReadOnlyDictionary<int, string> QuantityOptions { get; }

        private FixedOrDefaultFundDimensionOption<T> GetFundDimension<T>(IEnumerable<FundDimensionOption<T>> allowedValues,
                                                                         FundDimensionOption<T> defaultValue)
            where T : FundDimensionOption<T> {
            return new FixedOrDefaultFundDimensionOption<T>(allowedValues.IsSingle() ? allowedValues.Single() : null,
                                                            defaultValue);
        }
    

        private IReadOnlyDictionary<int, string> GetQuantityOptions() {
            var dict = new Dictionary<int, string>();
        
            var unitPriceText = _formatter.Number.FormatMoney(Price);

            if (Price != null) {
                for (var qty = 1; qty <= 10; qty++) {
                    var total = Price.Amount * qty;
                    var totalText = _formatter.Number.FormatMoney(total, Currency);

                    dict[qty] = $"{qty} @ {unitPriceText} = {totalText}";
                }
            }

            return dict;
        }
    }
}
