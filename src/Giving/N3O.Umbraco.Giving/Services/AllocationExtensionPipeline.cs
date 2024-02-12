using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Services;

class AllocationExtensionPipeline : IAllocationExtensionPipeline {
    private readonly IReadOnlyList<IAllocationExtensionBinder> _allocationExtensionBinders;
    
    public AllocationExtensionPipeline(IEnumerable<IAllocationExtensionBinder> allocationExtensionBinders) {
        _allocationExtensionBinders = allocationExtensionBinders.ToList();
    }
    
    public AllocationExtensionData Run(AllocationReq allocation) {
        var allocationExtensionData = new AllocationExtensionData();

        foreach (var allocationExtensionBinder in _allocationExtensionBinders.Where(x => x.CanBind(allocation))) {
            var data = allocationExtensionBinder.Bind(allocation);
            
            allocationExtensionData.Add(allocationExtensionBinder.Key, data);
        }

        return allocationExtensionData;
    }
}