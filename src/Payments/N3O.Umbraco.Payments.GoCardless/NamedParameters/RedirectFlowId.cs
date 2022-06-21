using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Payments.GoCardless.NamedParameters;

public class RedirectFlowId : NamedParameter<string> {
    public override string Name => "redirect_flow_id";
}
