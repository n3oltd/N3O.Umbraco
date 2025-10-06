using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Sync.Extensions.Models;

public class SyncDataReq {
    [Name("Data")]
    public object Data { get; set; }
    
    [Name("Shared Secret")]
    public string SharedSecret { get; set; }
}