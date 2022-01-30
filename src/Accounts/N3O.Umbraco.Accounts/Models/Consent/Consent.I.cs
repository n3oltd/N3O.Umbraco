using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public interface IConsent {
        IEnumerable<IConsentChoice> Choices { get; }
    }
}