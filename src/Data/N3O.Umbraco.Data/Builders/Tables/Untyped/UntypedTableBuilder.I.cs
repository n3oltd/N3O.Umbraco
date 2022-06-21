using N3O.Umbraco.Data.Models;
using System.Collections;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders;

public interface IUntypedTableBuilder {
    void AddCell(IColumnRange columnRange, Cell cell);
    void AddValue(IColumnRange columnRange, object value);
    void AddCells(IColumnRange columnRange, IEnumerable<Cell> cells);
    void AddValues(IColumnRange columnRange, IEnumerable values);
    void NextRow();

    ITable Build();
}
