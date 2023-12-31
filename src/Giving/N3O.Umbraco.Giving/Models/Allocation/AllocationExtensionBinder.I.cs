namespace N3O.Umbraco.Giving.Models;

public interface IAllocationExtensionBinder {
    object Bind(AllocationReq allocationReq);
}