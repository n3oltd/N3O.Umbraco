using Newtonsoft.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingData : Value {
    // TeamId, CampaignId, PageId, comment, should also probably store here (and in
    // the SQL DB which price handle they clicked if any (for stats purposes)

    [JsonConstructor]
    public CrowdfundingData() {
        
    }

    public CrowdfundingData(ICrowdfundingData crowdfundingData) {
        
    }
}