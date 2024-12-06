using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class ImportTemplateReq {
    [Name("Properties")]
    public IEnumerable<string> Properties { get; set; }
}