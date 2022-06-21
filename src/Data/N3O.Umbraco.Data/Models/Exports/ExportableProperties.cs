using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class ExportableProperties : Value {
    public ExportableProperties(IEnumerable<ExportableProperty> properties) {
        Properties = properties;
    }

    public IEnumerable<ExportableProperty> Properties { get; }
}
