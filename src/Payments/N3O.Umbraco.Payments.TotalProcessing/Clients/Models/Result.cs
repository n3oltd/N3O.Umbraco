using Newtonsoft.Json;
using System.Collections;

namespace Payments.TotalProcessing.Clients.Models;

public class Result {
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("parameterErrors")]
    public IEnumerable ParameterErrors { get; set; }
}
