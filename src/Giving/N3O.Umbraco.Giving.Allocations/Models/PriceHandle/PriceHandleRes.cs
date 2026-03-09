using N3O.Umbraco.Financial;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceHandleRes {
    public decimal Amount { get; set; }
    public Dictionary<string, MoneyRes> CurrencyValues { get; set; }
    public string Description { get; set; }
}
