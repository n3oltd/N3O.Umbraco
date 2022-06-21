using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart : Entity {
    public Currency Currency { get; private set; }
    public CartContents Donation { get; private set; }
    public CartContents RegularGiving { get; private set; }

    public bool IsEmpty() => Donation.IsEmpty() && RegularGiving.IsEmpty();
}
