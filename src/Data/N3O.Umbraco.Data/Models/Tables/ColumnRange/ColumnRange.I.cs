using N3O.Umbraco.Data.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public interface IColumnRange {
    void AddCells(int row, object cells);
    void AddValues(int row, object value);
    Cell GetCell(CellAddress address);
    IEnumerable<Column> GetColumns();
    DataType DataType { get; }
    int Order { get; }
}
