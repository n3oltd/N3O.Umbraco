using System;

namespace N3O.Umbraco.CrowdFunding.Models; 

class CrowdfundingAllocationReqValidator : ICrowdfundingAllocationReqValidator {
    public bool IsValid(Guid pageId, string pageUrl, string comment, string name, bool hideName) {
        return true;
    }
}