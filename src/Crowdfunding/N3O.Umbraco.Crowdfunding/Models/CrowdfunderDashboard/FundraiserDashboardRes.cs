using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserDashboardRes {
    public IEnumerable<FundraiserDashboardPageRes> Entries { get; set; }
    public long CurrentPage { get; set; }
    public bool HasMoreEntries { get; set; }
}