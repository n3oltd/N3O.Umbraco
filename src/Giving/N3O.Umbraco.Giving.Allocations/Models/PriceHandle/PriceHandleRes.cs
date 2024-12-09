using System.Collections.Generic;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceHandleRes {
    public decimal Amount { get; set; }
    public Dictionary<string, MoneyRes> CurrencyValues { get; set; }
    public string Description { get; set; }
}
