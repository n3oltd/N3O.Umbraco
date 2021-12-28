using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Giving.Pricing.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart {
    public class CartValidator : ICartValidator {
        private readonly IPricing _pricing;

        public CartValidator(IPricing pricing) {
            _pricing = pricing;
        }
    
        public bool IsValid(Currency currentCurrency, DonationCart cart) {
            try {
                var isValid = currentCurrency == cart.Currency &&
                              ContentsAreValid(currentCurrency, cart.Single) &&
                              ContentsAreValid(currentCurrency, cart.Regular);

                return isValid;
            } catch {
                return false;
            }
        }

        private bool ContentsAreValid(Currency currency, CartContents cartContents) {
            foreach (var allocation in cartContents.OrEmpty(x => x.Allocations)) {
                if (!AllocationIsValid(currency, allocation)) {
                    return false;
                }
            }

            return true;
        }

        private bool AllocationIsValid(Currency currency, Allocation allocation) {
            if (allocation.Type == AllocationTypes.Fund) {
                var fund = allocation.Fund;

                if (!fund.HasValue(x => x.DonationItem)) {
                    return false;
                }

                if (!FundDimensionIsValid(fund.DonationItem.Dimension1Options, allocation.Dimension1) ||
                    !FundDimensionIsValid(fund.DonationItem.Dimension2Options, allocation.Dimension2) ||
                    !FundDimensionIsValid(fund.DonationItem.Dimension3Options, allocation.Dimension3) ||
                    !FundDimensionIsValid(fund.DonationItem.Dimension4Options, allocation.Dimension4)) {
                    return false;
                }

                if (allocation.Value.IsZero() && !fund.DonationItem.Free) {
                    return false;
                }

                if (allocation.Fund.DonationItem.HasPrice()) {
                    var currencyPrice = _pricing.InCurrency(fund.DonationItem, currency);

                    if (currencyPrice != allocation.Value) {
                        return false;
                    }
                }
            } else if (allocation.Type == AllocationTypes.Sponsorship) {
                var sponsorship = allocation.Sponsorship;
            
                if (!sponsorship.HasValue(x => x.Scheme)) {
                    return false;
                }
            
                if (!FundDimensionIsValid(sponsorship.Scheme.Dimension1Options, allocation.Dimension1) ||
                    !FundDimensionIsValid(sponsorship.Scheme.Dimension2Options, allocation.Dimension2) ||
                    !FundDimensionIsValid(sponsorship.Scheme.Dimension3Options, allocation.Dimension3) ||
                    !FundDimensionIsValid(sponsorship.Scheme.Dimension4Options, allocation.Dimension4)) {
                    return false;
                }
            } else {
                throw UnrecognisedValueException.For(allocation.Type);
            }

            return true;
        }

        private bool FundDimensionIsValid<T>(IEnumerable<T> allowed, T value) {
            return allowed.None() || allowed.Contains(value);
        }
    }
}