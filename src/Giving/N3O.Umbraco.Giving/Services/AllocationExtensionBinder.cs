using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Giving.Models;

public abstract class AllocationExtensionBinder<TReq, TModel> : IAllocationExtensionBinder {
    private readonly IJsonProvider _jsonProvider;

    protected AllocationExtensionBinder(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    /*public bool CanBind(AllocationReq allocationReq) {
        return allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key);
    }*/

    public object Bind(AllocationReq allocationReq) {
        var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

        return Bind(extensionDataReq);
    }

    protected abstract TModel Bind(TReq req);
    public abstract bool CanBind(AllocationReq allocationReq);
    
    public abstract string Key { get; }
}