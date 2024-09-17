using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class NestedSchemaRes {
    public IEnumerable<NestedSchemaItemRes> Items { get; set; }
}