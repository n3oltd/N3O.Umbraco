using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Data.NamedParameters;

public class ContentId : NamedParameter<Guid> {
    public override string Name => "contentId";
}
