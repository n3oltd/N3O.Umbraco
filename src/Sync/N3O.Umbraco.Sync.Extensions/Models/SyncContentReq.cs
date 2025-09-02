using System;

namespace N3O.Umbraco.Sync.Extensions.Models;

public class SyncContentReq {
    public Guid? RequestId { get; set; }
    
    public Guid? ContentId { get; set; }
    
    public string ServerAlias { get; set; }
}