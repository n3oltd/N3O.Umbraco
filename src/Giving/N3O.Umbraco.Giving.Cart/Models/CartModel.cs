using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Cart.Models {
    public class CartModel {
        private static readonly string CheckoutPageAlias = "CheckoutPage";
        private static readonly string DonatePageAlias = "DonatePage";
        
        private readonly IFormatter _formatter;

        public CartModel(IFormatter formatter,
                         IContentCache contentCache,
                         Currency currency,
                         CartContents single,
                         CartContents regular,
                         bool checkoutView) {
            _formatter = formatter;
            Currency = currency;
            Single = single;
            Regular = regular;
            CheckoutView = checkoutView;
            TotalText = GetTotalText(single, regular);
            TotalItems = single.Allocations.Count() + regular.Allocations.Count();
            CheckoutUrl = contentCache.Single(CheckoutPageAlias)?.Url();
            DonateUrl = contentCache.Single(DonatePageAlias)?.Url();
        }

        public Currency Currency { get; }
        public CartContents Single { get; }
        public CartContents Regular { get; }
        public bool CheckoutView { get; }
        public string TotalText { get; }
        public int TotalItems { get; }
        public string CheckoutUrl { get; }
        public string DonateUrl { get; }

        public bool IsEmpty() => Single.IsEmpty() && Regular.IsEmpty();
    
        private string GetTotalText(CartContents single, CartContents regular) {
            var totals = new List<string>();

            if (!single.Total.IsZero()) {
                totals.Add(_formatter.Text.Format<Strings>(s => s.SingleTotal,
                                                           _formatter.Number.FormatMoney(single.Total)));
            }
        
            if (!regular.Total.IsZero()) {
                totals.Add(_formatter.Text.Format<Strings>(s => s.RegularTotal,
                                                           _formatter.Number.FormatMoney(regular.Total)));
            }

            return string.Join(" + ", totals);
        }

        public class Strings : CodeStrings {
            public string SingleTotal => "{0}";
            public string RegularTotal => "{0} / month";
        }
    }
}