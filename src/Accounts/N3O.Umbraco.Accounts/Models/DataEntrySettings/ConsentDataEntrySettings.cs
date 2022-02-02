using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentDataEntrySettings : Value {
        public ConsentDataEntrySettings(IEnumerable<ConsentOption> consentOptions) {
            ConsentOptions = consentOptions;
        }

        public IEnumerable<ConsentOption> ConsentOptions { get; }
    }
}