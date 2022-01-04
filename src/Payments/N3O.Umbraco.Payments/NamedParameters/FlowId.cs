using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Payments.NamedParameters {
    public class FlowId : NamedParameter<Guid> {
        public override string Name => "flowId";
    }
}