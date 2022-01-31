using N3O.Umbraco.Financial;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Pricing.Models {
    public class PriceRes : IPrice {
        public MoneyRes Value { get; set; }
        public bool Locked { get; set; }

        [JsonIgnore]
        decimal IPrice.Amount => Value.Amount;
    }
}