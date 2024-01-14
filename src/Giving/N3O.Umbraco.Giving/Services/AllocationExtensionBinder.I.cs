namespace N3O.Umbraco.Giving.Models;

public interface IAllocationExtensionBinder {
    string Key { get; }
    
    bool CanBind(AllocationReq allocationReq);
    object Bind(AllocationReq allocationReq);
}