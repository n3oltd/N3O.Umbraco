using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class DataProperties : Value {
    public DataProperties(IEnumerable<DataProperty> properties) {
        Properties = properties;
    }

    public IEnumerable<DataProperty> Properties { get; }
}
