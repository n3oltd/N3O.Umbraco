using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Webhooks.NamedParameters;

public class HookId : NamedParameter<string> {
    public override string Name => "hookId";
}
