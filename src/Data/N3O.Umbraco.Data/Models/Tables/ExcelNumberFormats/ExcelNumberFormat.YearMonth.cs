namespace N3O.Umbraco.Data.Models {
    public class YearMonthExcelNumberFormat : ExcelNumberFormat {
        public YearMonthExcelNumberFormat() {
            Pattern = "mmm yyyy";
        }
    }
}
