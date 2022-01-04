using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart : Entity {
        public Currency Currency { get; private set; }
        public CartContents Single { get; private set; }
        public CartContents Regular { get; private set; }

        public bool IsEmpty() => Single.IsEmpty() && Regular.IsEmpty();
    }
}