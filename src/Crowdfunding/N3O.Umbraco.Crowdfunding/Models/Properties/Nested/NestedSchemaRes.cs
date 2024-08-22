using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedSchemaRes {
    public IEnumerable<NestedSchemaItemRes> Items { get; set; }
}