using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class BlockListSchemaRes {
    public IEnumerable<BlockListSchemaItemRes> Items { get; set; }
}