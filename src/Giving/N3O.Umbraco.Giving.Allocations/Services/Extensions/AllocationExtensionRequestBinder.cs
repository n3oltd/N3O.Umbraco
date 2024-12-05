using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations;

public abstract class AllocationExtensionRequestBinder<TReq, TModel> : IAllocationExtensionRequestBinder {
    private readonly IJsonProvider _jsonProvider;

    protected AllocationExtensionRequestBinder(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    public void Bind(IDictionary<string, JToken> src, IDictionary<string, JToken> dest) {
        var req = src.Get<TReq>(_jsonProvider, Key);

        var model = Bind(req);
        
        dest.Set(_jsonProvider, Key, model);
    }

    protected abstract TModel Bind(TReq req);
    
    public abstract string Key { get; }
}