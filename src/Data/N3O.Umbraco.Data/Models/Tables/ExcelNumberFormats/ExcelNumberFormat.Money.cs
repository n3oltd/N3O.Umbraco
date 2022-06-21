using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Data.Models;

public class MoneyExcelNumberFormat : ExcelNumberFormat {
    public MoneyExcelNumberFormat(Currency currency) {
        if (currency != null) {
            Pattern = $"{currency.Symbol}#,##0.00";
        } else {
            Pattern = "#,##0.00";
        }
    }
}
