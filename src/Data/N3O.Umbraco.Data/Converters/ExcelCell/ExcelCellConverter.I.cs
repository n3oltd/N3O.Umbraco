using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters {
    public interface IExcelCellConverter {
        public ExcelCell GetExcelCell(Column column, Cell cell);
    }

    public interface IExcelCellConverter<T> : IExcelCellConverter {
        public ExcelCell<T> GetExcelCell(Column column, Cell<T> cell);
    }
}