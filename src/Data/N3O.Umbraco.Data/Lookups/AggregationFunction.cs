using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Lookups;

public class AggregationFunction : NamedLookup {
    public AggregationFunction(string id,
                               string name,
                               IEnumerable<DataType> dataTypes,
                               string titleSuffix,
                               bool preservesType = true,
                               DataType resultType = null)
        : base(id, name) {
        DataTypes = dataTypes;
        TitleSuffix = titleSuffix;
        PreservesType = preservesType;
        ResultType = resultType;
    }
    
    public IEnumerable<DataType> DataTypes { get; }
    public string TitleSuffix { get; }
    public bool PreservesType { get; }
    public DataType ResultType { get; }
}

[StaticLookups]
public class AggregationFunctions : StaticLookupsCollection<AggregationFunction> {
    public static readonly AggregationFunction Average = new("average", "Average", OurDataTypes.NumericTypes, "Average");
    public static readonly AggregationFunction Count = new("count", "Count", OurDataTypes.GetAllTypes(), "Count", false, OurDataTypes.Integer);
    public static readonly AggregationFunction Min = new("min", "Min", OurDataTypes.OrdinalTypes, "Minimum");
    public static readonly AggregationFunction Max = new("max", "Max", OurDataTypes.OrdinalTypes, "Maximum");
    public static readonly AggregationFunction Sum = new("sum", "Total", OurDataTypes.NumericTypes, "Total");
}
