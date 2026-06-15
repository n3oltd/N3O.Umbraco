using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class BlockListSchemaItemRes {
    public string ContentTypeAlias { get; set; }
    public IEnumerable<BlockListSchemaPropertyRes> Properties { get; set; }
}