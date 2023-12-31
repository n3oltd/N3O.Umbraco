using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Giving.Models;

public abstract class AllocationExtensionBinder<TReq, TModel> : IAllocationExtensionBinder {
    private readonly IJsonProvider _jsonProvider;

    protected AllocationExtensionBinder(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }
    
    public object Bind(AllocationReq allocationReq) {
        // Notice how similar pattern is here to above, hence we may want to use extension method or some other way
        // to avoid code duplication
        if (allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key)) {
            // TODO Fix this line, we want to convert the JObject to TReq, lots of places in Karakoram code where we do
            // this just can't remember how
            var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

            return Bind(extensionDataReq);
        } else {
            return null;   
        }
    }

    protected abstract TModel Bind(TReq req);
    
    protected abstract string Key { get; }
}

// TODO When we are passing the allocationReq into the allocation model constructor, before we do this we should
// inject the IEnumerable<IAllocationExtensionBinder> and use this to run all the custom binders to get the strongly
// types model for allocation extension data.