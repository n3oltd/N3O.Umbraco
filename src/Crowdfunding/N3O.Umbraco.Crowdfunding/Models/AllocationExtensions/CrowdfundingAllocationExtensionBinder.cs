using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingAllocationExtensionBinder : AllocationExtensionBinder<CrowdfundingDataReq, CrowdfundingData> {
    public CrowdfundingAllocationExtensionBinder(IJsonProvider jsonProvider) : base(jsonProvider) { }
    
    protected override CrowdfundingData Bind(CrowdfundingDataReq req) {
        return new CrowdfundingData(req);
    }
    
    protected override string Key => "crowdfunding";
}