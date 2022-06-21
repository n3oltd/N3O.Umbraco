using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class DonationFormRes {
    public string Title { get; set; }
    public IEnumerable<DonationOptionRes> Options { get; set; }
}
