using N3O.Umbraco.Financial;
using System;
using EngageCurrency = N3O.Umbraco.Crm.Engage.Clients.Currency;

namespace N3O.Umbraco.Crm.Engage.Extensions;

public static class CurrencyExtensions {
    public static EngageCurrency ToEngageCurrency(this Currency currency) {
        return (EngageCurrency) Enum.Parse(typeof(EngageCurrency), currency.Id, true);
    }
}