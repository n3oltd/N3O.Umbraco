using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedSchemaItemRes {
    public string ContentTypeAlias { get; set; }
    public IEnumerable<NestedSchemaPropertyRes> Properties { get; set; }
}