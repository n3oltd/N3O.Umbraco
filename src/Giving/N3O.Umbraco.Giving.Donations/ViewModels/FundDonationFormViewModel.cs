using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Donations.Content;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Giving.Pricing.Extensions;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.ViewModels {
    public class FundDonationFormViewModel {
        private readonly IForexConverter _forexConverter;
        private readonly IUmbracoMapper _mapper;
        private readonly IFormatter _formatter;

        public FundDonationFormViewModel(IForexConverter forexConverter,
                                         IPricing pricing,
                                         IUmbracoMapper mapper,
                                         ICurrencyAccessor currencyAccessor,
                                         IFormatter formatter,
                                         FundDonationOption fundOption,
                                         DonationType donationType) {
            _forexConverter = forexConverter;
            _mapper = mapper;
            _formatter = formatter;

            DonationItem = fundOption.DonationItem;
            Type = donationType;
            Currency = currencyAccessor.GetCurrency();
            FundDimension1 = GetFundDimension(DonationItem.Dimension1Options, DonationItem.DefaultFundDimension1());
            FundDimension2 = GetFundDimension(DonationItem.Dimension2Options, DonationItem.DefaultFundDimension2());
            FundDimension3 = GetFundDimension(DonationItem.Dimension3Options, DonationItem.DefaultFundDimension3());
            FundDimension4 = GetFundDimension(DonationItem.Dimension4Options, DonationItem.DefaultFundDimension4());
            Price = DonationItem.HasPrice() ? _mapper.Map<Money, MoneyRes>(pricing.InCurrency(DonationItem, Currency)) : null;
            PriceHandles = GetPriceHandles(fundOption, donationType).ToList();
            QuantityOptions = GetQuantityOptions(fundOption);
        }

        public DonationItem DonationItem { get; }
        public DonationType Type { get; }
        public Currency Currency { get; }
        public FixedOrDefaultFundDimensionOption FundDimension1 { get; }
        public FixedOrDefaultFundDimensionOption FundDimension2 { get; }
        public FixedOrDefaultFundDimensionOption FundDimension3 { get; }
        public FixedOrDefaultFundDimensionOption FundDimension4 { get; }
        public MoneyRes Price { get; }
        public IReadOnlyList<PriceHandleViewModel> PriceHandles { get; }
        public IReadOnlyDictionary<int, string> QuantityOptions { get; }

        private IEnumerable<PriceHandleViewModel> GetPriceHandles(FundDonationOption fundOption, DonationType donationType) {
            var priceHandles = fundOption.GetPriceHandles(donationType);
        
            foreach (var (priceHandle, index) in priceHandles.SelectWithIndex()) {
                var currencyPrice = _forexConverter.BaseToQuote()
                                                   .ToCurrency(Currency)
                                                   .ConvertAsync(priceHandle.Amount)
                                                   .GetAwaiter()
                                                   .GetResult();

                var viewModel = new PriceHandleViewModel(index,
                                                         _mapper.Map<Money, MoneyRes>(currencyPrice.Quote),
                                                         priceHandle.Description);

                yield return viewModel;
            }
        }
    
        private FixedOrDefaultFundDimensionOption GetFundDimension(IEnumerable<FundDimensionOption> allowedValues,
                                                                   FundDimensionOption defaultValue) {
            return new FixedOrDefaultFundDimensionOption(allowedValues.IsSingle() ? allowedValues.Single() : null,
                                                         defaultValue);
        }

        private IReadOnlyDictionary<int, string> GetQuantityOptions(FundDonationOption fundOption) {
            var dict = new Dictionary<int, string>();
        
            if (fundOption.ShowQuantity && Price != null) {
                var unitPriceText = _formatter.Number.FormatMoney(Price);
            
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
