using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Models {
    public class CartContents : Value {
        [JsonConstructor]
        public CartContents(GivingType type, IEnumerable<Allocation> allocations, Money total) {
            Type = type;
            Allocations = allocations;
            Total = total;
        }

        public CartContents(Currency currency, GivingType type, IEnumerable<Allocation> allocations)
            : this(type,
                   allocations,
                   allocations.HasAny() ? allocations.Select(x => x.Value).Sum() : currency.Zero()) { }

        public GivingType Type { get; }
        public IEnumerable<Allocation> Allocations { get; }
        public Money Total { get; }

        public bool IsEmpty() => Allocations.None();

        public static CartContents Create(Currency currency, GivingType type) {
            return new CartContents(type, Enumerable.Empty<Allocation>(), currency.Zero());
        }
    }
}