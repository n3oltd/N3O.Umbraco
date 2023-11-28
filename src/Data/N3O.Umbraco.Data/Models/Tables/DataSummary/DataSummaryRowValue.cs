namespace N3O.Umbraco.Data.Models; 

public class DataSummaryRowValue {
    public DataSummaryRowValue(object value, ExcelNumberFormat formatting) {
        Value = value;
        Formatting = formatting;
    }
    
    public object Value { get; set; }
    public ExcelNumberFormat Formatting { get; set; }
}