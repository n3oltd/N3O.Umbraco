using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Forex.Currencylayer {
    public class ApiResponse {
        public bool Success { get; set; }
        public long Timestamp { get; set; }
        public Dictionary<string, decimal> Quotes { private get; set; }
        public decimal Rate => Quotes.Single().Value;
    }
}
