using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Services; 

public interface IAllocationExtensionPipeline {
    public AllocationExtensionData Run(AllocationReq allocation);
}