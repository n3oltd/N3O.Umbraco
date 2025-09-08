using System;

namespace N3O.Umbraco.Parameters;

public class ContentId : NamedParameter<Guid> {
    public override string Name => "contentId";
}