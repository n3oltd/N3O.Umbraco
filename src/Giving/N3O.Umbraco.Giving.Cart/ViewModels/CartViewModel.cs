using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Localization;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Cart.ViewModels;

public class CartViewModel {
    private readonly IFormatter _formatter;

    public CartViewModel(IFormatter formatter,
                         Currency currency,
                         CartContents single,
                         CartContents regular,
                         bool checkoutView,
                         string totalText) {
        _formatter = formatter;
        Currency = currency;
        Single = single;
        Regular = regular;
        CheckoutView = checkoutView;
        TotalText = totalText;
    }

    public Currency Currency { get; }
    public CartContents Single { get; }
    public CartContents Regular { get; }
    public bool CheckoutView { get; }
    public string TotalText { get; }

    public bool IsEmpty() => Single.IsEmpty() && Regular.IsEmpty();
    
    private string GetTotalText(DonationCart cart) {
        var totals = new List<string>();

        if (!cart.Single.Total.IsZero()) {
            totals.Add(_formatter.Text.Format<Strings>(s => s.SingleTotal,
                                                       _formatter.Number.FormatMoney(cart.Single.Total)));
        }
        
        if (!cart.Regular.Total.IsZero()) {
            totals.Add(_formatter.Text.Format<Strings>(s => s.RegularTotal,
                                                       _formatter.Number.FormatMoney(cart.Regular.Total)));
        }

        return string.Join(" + ", totals);
    }

    public class Strings : CodeStrings {
        public string SingleTotal => "{0}";
        public string RegularTotal => "{0} / month";
    }
}