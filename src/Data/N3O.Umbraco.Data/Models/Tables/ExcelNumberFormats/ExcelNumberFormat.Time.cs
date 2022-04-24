using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Models {
    public class TimeExcelNumberFormat : ExcelNumberFormat {
        public TimeExcelNumberFormat(TimeFormat timeFormat) {
            Pattern = "HH:mm";

            if (timeFormat.HasMeridiem) {
                Pattern += " am/pm";
            }
        }
    }
}