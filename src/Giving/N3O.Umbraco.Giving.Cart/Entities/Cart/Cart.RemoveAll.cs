using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public void RemoveAll(GivingType givingType) {
            ReplaceContents(givingType, _ => CartContents.Create(Currency, givingType));
        }
    }
}