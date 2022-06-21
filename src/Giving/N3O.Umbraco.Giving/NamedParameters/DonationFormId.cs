using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Giving.NamedParameters;

public class DonationFormId : NamedParameter<Guid> {
    public override string Name => "donationFormId";
}
