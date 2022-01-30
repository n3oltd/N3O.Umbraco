using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups {
    public class ConsentResponse : NamedLookup {
        public ConsentResponse(string id, string name, bool? value) : base(id, name) {
            Value = value;
        }
        
        public bool? Value { get; }
    }
    
    public class ConsentResponses : StaticLookupsCollection<ConsentResponse> {
        public static readonly ConsentResponse NoResponse = new("noResponse", "No Response", null);
        public static readonly ConsentResponse OptIn = new("optIn", "Opt-In", true);
        public static readonly ConsentResponse OptOut = new("optOut", "Opt-Out", false);
    }
}