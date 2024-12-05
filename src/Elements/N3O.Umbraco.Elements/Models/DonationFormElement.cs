using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class DonationFormElement {
    public Guid Id { get; set; }
    public Guid CheckoutProfileId { get; set; }
    public IEnumerable<DonationCategoryPartial> RootCategories { get; set; }
}