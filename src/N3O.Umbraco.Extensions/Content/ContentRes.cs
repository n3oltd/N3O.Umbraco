using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public class ContentRes {
    public ContentRes() {
        Properties = new Dictionary<string, object>();
    }

    public int Id { get; set; }
    public Guid Key { get; set; }
    public string Url { get; set; }
    public int Level { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CreatorName { get; set; }
    public string WriterName { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public int SortOrder { get; set; }
    public string ContentTypeAlias { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> Properties { get; set; }
}