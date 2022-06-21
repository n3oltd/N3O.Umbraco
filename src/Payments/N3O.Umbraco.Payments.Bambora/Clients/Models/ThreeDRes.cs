using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class ThreeDRes {
    [JsonProperty("3d_session_data")]
    public string ThreeDSessionData { get; set; }

    [JsonProperty("contents")]
    public string Contents { get; set; }

    [JsonProperty("links")]
    public IEnumerable<Link> Links { get; set; }

    [JsonIgnore]
    public string ChallengeUrl => Links.Single(x => x.Rel.EqualsInvariant("continue")).Href;

    [JsonIgnore]
    public string DecodedContents => HttpUtility.UrlDecode(Contents);
}
