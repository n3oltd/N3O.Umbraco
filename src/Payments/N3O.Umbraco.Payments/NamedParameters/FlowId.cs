using N3O.Umbraco.Entities;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Payments.NamedParameters;

public class FlowId : NamedParameter<EntityId> {
    public override string Name => "flowId";
}
