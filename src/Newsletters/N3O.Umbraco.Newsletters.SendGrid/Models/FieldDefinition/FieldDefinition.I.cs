using N3O.Umbraco.Data.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid.Models;

public interface IFieldDefinition {
    string Id { get; }
    string Name { get; }
    bool Supported { get; }
    bool Reserved { get; }
    DataType Type { get; }
    IEnumerable<DataType> SourceTypes { get; }
}