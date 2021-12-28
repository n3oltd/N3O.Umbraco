using N3O.Umbraco.Giving.Cart.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Cart.Commands;

public class RemoveFromCartCommand : Request<None, None> {
    public AllocationIndex AllocationIndex { get; }

    public RemoveFromCartCommand(AllocationIndex allocationIndex) {
        AllocationIndex = allocationIndex;
    }
}