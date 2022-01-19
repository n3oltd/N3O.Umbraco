using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class TelephoneRes : ITelephone {
        public Country Country { get; set; }
        public string Number { get; set; }
    }
}
