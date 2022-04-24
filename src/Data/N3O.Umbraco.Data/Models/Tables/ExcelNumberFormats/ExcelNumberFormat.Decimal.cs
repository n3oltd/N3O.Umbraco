namespace N3O.Umbraco.Data.Models {
    public class DecimalExcelNumberFormat : ExcelNumberFormat {
        public DecimalExcelNumberFormat() {
            Pattern = "#,##0.00";
        }
    }
}