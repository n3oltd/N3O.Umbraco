namespace N3O.Umbraco.Giving.Models;

public interface IAllocationExtensionBinder {
    string Key { get; }
    object Bind(AllocationReq allocationReq);
}