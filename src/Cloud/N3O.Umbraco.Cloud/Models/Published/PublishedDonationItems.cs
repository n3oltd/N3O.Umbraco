using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedDonationItems {
    public IEnumerable<PublishedDonationItem> DonationItems { get; set; }
}