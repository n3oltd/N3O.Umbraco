using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class DonationFormRes {
    public string Title { get; set; }
    public IEnumerable<DonationOptionRes> Options { get; set; }
}