using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Cart.Models;

public class BulkAddToCartReq {
    [Name("Items")]
    public IEnumerable<AddToCartReq> Items { get; set; }
}