using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Sync.Extensions.Models;

public class SyncContentReq {
    [Name("Request ID")]
    public Guid? RequestId { get; set; }
    
    [Name("Content ID")]
    public Guid? ContentId { get; set; }
    
    [Name("Server Alias")]
    public string ServerAlias { get; set; }
}