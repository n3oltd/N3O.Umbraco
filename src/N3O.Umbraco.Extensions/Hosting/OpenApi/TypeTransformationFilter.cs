using N3O.Umbraco.Extensions;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Hosting;

public abstract class TypeTransformationFilter : ISchemaProcessor  {
    private SchemaProcessorContext _context;

    public void Process(SchemaProcessorContext context) {
        _context = context;

        DoProcess(context);
    }

    protected void ModelAsEnum(Type propertyType, IEnumerable<string> values, string example = null) {
        var typeName = propertyType.Name;
        string description = null;
        
        if (values.HasAny()) {
            values.Do(x => _context.Schema.Enumeration.Add(x));

            _context.Schema.ExtensionData = new Dictionary<string, object> {
                {
                    "x-ms-enum",
                    new {
                        name = typeName,
                        modelAsString = true
                    }
                }
            };
            
            description = $"One of {string.Join(", ", values.Select(x => x.Quote()))}";
        }

        ModelAsString(example, description);
    }

    protected void ModelAsInt(long? example = null, string description = null) {
        ModelAsType(JsonObjectType.Integer, example?.ToString(), description);
    }

    protected void ModelAsFloatDouble(double? example = null, string description = null) {
        ModelAsType(JsonObjectType.Number, example?.ToString(), description);
    }

    protected void ModelAsString(string example = null, string description = null) {
        ModelAsType(JsonObjectType.String, example, description);
    }

    private void ModelAsType(JsonObjectType type, string example, string description) {
        _context.Schema.Type = type;

        if (description.HasValue()) {
            _context.Schema.Description = description;
        }

        if (example.HasValue() &&
            !_context.Schema.Example.HasValue()) {
            _context.Schema.Example = example;
        } 
        
        _context.Schema.Format = null;
        _context.Schema.AllOf.Clear();
        _context.Schema.Items.Clear();
        _context.Schema.Properties.Clear();
    }
    
    protected abstract void DoProcess(SchemaProcessorContext context);
}
