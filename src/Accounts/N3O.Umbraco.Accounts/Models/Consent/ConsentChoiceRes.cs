using N3O.Umbraco.Accounts.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentChoiceRes : IConsentChoice {
        public ConsentChannel Channel { get; set; }
        public ConsentCategory Category { get; set; }
        public ConsentResponse Response { get; set; }
    }
}