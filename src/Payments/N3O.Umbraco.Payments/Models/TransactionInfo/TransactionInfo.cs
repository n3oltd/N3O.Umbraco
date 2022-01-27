using N3O.Umbraco.Counters;

namespace N3O.Umbraco.Payments.Entities {
    public class TransactionInfo {
        public TransactionInfo(Reference reference, string description) {
            Reference = reference;
            Description = description;
        }
        
        public Reference Reference { get; }
        public string Description { get;  }
    }
}