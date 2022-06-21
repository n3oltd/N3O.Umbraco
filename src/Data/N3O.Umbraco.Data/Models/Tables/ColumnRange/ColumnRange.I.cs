using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public interface IColumnRange {
    void AddCells(int row, object cells);
    void AddValues(int row, object value);
    Cell GetCell(CellAddress address);
    IEnumerable<Column> GetColumns();
    int Order { get; }
}
