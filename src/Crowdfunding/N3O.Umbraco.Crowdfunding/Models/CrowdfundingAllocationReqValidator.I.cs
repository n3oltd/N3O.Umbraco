using System;

namespace N3O.Umbraco.CrowdFunding.Models; 

public interface ICrowdfundingAllocationReqValidator {
    bool IsValid(Guid pageId, string pageUrl, string comment, string name, bool hideName);
}