using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class ResultDetails {
    [JsonProperty("ExtendedDescription")]
    public string ExtendedDescription { get; set; }

    [JsonProperty("ConnectorTxID1")]
    public string ConnectorTxID1 { get; set; }

    [JsonProperty("ConnectorTxID3")]
    public string ConnectorTxID3 { get; set; }

    [JsonProperty("ConnectorTxID2")]
    public string ConnectorTxID2 { get; set; }

    [JsonProperty("AcquirerResponse")]
    public string AcquirerResponse { get; set; }
}
