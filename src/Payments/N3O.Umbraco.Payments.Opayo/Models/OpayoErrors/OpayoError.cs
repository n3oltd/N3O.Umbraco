namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoError {
        public string ClientMessage { get; set; }
        public int? Code { get; set; }
        public string Description { get; set; }
        public string Property { get; set; }
        public int? StatusCode { get; set; }
        public string StatusDetail { get; set; }
        public string TransactionId { get; set; }
    }
}