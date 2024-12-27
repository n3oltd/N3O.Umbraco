using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cdn.Cloudflare.Clients;

public class ApiUploadRes {
    [JsonProperty("result")]
    public ApiUploadResult Result { get; set; }
    
    [JsonProperty("success")]
    public bool Success { get; set; }
    
    [JsonProperty("errors")]
    public IEnumerable<ApiErrorRes> Errors { get; set; }
}