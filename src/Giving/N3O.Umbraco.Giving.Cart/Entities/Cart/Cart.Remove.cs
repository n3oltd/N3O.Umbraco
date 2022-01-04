using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public void Remove(DonationType donationType, int allocationIndex) {
            ReplaceContents(donationType, c => RemoveContents(c, allocationIndex));
        }
        
        private CartContents RemoveContents(CartContents contents, int allocationIndex) {
            if (allocationIndex < 0 || allocationIndex >= contents.Allocations.Count()) {
                throw new IndexOutOfRangeException($"{nameof(allocationIndex)} is out of range");
            }

            var allocations = contents.Allocations.ExceptAt(allocationIndex);

            return new CartContents(Currency, contents.Type, allocations);
        }
    }
}