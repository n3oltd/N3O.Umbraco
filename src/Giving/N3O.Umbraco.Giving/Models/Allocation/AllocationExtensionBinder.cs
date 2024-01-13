using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Giving.Models;

public abstract class AllocationExtensionBinder<TReq, TModel> : IAllocationExtensionBinder {
    private readonly IJsonProvider _jsonProvider;

    protected AllocationExtensionBinder(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }
    
    public object Bind(AllocationReq allocationReq) {
        if (allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key)) {
            var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

            return Bind(extensionDataReq);
        } else {
            return null;   
        }
    }

    protected abstract TModel Bind(TReq req);
    public abstract string Key { get; }
}