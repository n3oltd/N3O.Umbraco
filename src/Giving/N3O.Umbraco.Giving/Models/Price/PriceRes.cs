using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models {
    public class PriceRes : IPrice {
        public decimal Amount { get; set; }
        public bool Locked { get; set; }
    }
}