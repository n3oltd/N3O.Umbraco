using N3O.Umbraco.Giving.Donations.Content;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations {
    public interface IDonationForms {
        IReadOnlyList<DonationFormContent> All();
        DonationOptionContent GetOption(Guid id);
    }
}
