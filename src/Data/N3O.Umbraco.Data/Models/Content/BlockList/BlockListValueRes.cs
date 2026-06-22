using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class BlockListValueRes {
    public IEnumerable<BlockListItemRes> Items { get; set; }
    public BlockListSchemaRes Schema { get; set; }
    public BlockListConfigurationRes Configuration { get; set; }
}