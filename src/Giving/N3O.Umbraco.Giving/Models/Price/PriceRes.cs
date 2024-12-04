using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class PriceRes : IPrice {
    public decimal Amount { get; set; }
    public Dictionary<string, MoneyRes> CurrencyValues { get; set; }
    public bool Locked { get; set; }
}
