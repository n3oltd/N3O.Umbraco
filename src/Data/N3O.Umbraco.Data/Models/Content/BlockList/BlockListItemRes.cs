using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class BlockListItemRes {
    public string ContentTypeAlias { get; set; }
    public IEnumerable<ContentPropertyValueRes> Properties { get; set; }
}