using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid.Models;

public class FieldDefinition : Value, IFieldDefinition {
    public FieldDefinition(string id,
                           string name,
                           bool supported,
                           bool reserved,
                           DataType type,
                           IEnumerable<DataType> sourceTypes) {
        Id = id;
        Name = name;
        Supported = supported;
        Reserved = reserved;
        Type = type;
        SourceTypes = sourceTypes.OrEmpty();
    }

    public string Id { get; }
    public string Name { get; }
    public bool Supported { get; }
    public bool Reserved { get; }
    public DataType Type { get; }
    public IEnumerable<DataType> SourceTypes { get; }
    
    public static FieldDefinition ForUnsupported(string id, string name, bool reserved) {
        return new FieldDefinition(id, name, false, reserved, null, null);
    }

    public static FieldDefinition ForSupported(string id,
                                               string name,
                                               bool reserved,
                                               DataType dataType,
                                               IEnumerable<DataType> sourceDataTypes) {
        return new FieldDefinition(id, name, true, reserved, dataType, sourceDataTypes);
    }
}