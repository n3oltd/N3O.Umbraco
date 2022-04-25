using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Models {
    public class CellValueColumnHeading : IColumnHeading {
        public string GetText(IFormatter formatter, int? columnIndex, Cell cell) {
            return cell.ToString(formatter);
        }
    }
}