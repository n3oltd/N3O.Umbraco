using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models; 

public class DataSummaryRow {
    public DataSummaryRow(string label, 
                          IEnumerable<DataSummaryRowValue> values) {
        Label = label;
        Values = values;
    }

    public string Label { get; }
    public IEnumerable<DataSummaryRowValue> Values { get; }
}