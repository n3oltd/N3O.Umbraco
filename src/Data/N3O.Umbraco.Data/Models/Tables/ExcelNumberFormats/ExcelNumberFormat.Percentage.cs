namespace N3O.Umbraco.Data.Models; 

public class PercentageExcelNumberFormat : ExcelNumberFormat {
    public PercentageExcelNumberFormat() {
        Pattern = "#,##0.00%";
    }
}