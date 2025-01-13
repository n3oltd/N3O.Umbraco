using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserPageResultsPage {
    public IEnumerable<FundraiserPageRes> Entries { get; set; }
    public int CurrentPage { get; set; }
    public bool HasMoreEntries { get; set; }
}