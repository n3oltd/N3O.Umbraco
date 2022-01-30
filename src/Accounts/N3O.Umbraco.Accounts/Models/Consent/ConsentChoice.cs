using N3O.Umbraco.Accounts.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentChoice : Value, IConsentChoice {
        [JsonConstructor]
        public ConsentChoice(ConsentChannel channel, ConsentCategory category, ConsentResponse response) {
            Channel = channel;
            Category = category;
            Response = response;
        }

        public ConsentChoice(IConsentChoice choice) : this(choice.Channel, choice.Category, choice.Response) { }
        
        public ConsentChannel Channel { get; }
        public ConsentCategory Category { get; }
        public ConsentResponse Response { get; }
    }
}