using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDashboardRes {
    public IEnumerable<CrowdfunderDashboardEntryRes> Entries { get; set; }
    public long CurrentPage { get; set; }
    public bool HasMoreEntries { get; set; }
}