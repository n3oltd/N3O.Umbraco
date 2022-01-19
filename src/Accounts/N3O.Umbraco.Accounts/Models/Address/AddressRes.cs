using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class AddressRes : IAddress {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Locality { get; set; }
        public string AdministrativeArea { get; set; }
        public string PostalCode { get; set; }
        public Country Country { get; set; }
    }
}
