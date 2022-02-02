using N3O.Umbraco.Accounts.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentOption : Value {
        public ConsentOption(ConsentChannel channel, IEnumerable<ConsentCategory> categories, string statement) {
            Channel = channel;
            Categories = categories;
            Statement = statement;
        }

        public ConsentChannel Channel { get; }
        public IEnumerable<ConsentCategory> Categories { get; }
        public string Statement { get; }
    }
}