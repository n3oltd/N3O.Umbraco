using N3O.Umbraco.Data.Models;
using System.Collections;

namespace N3O.Umbraco.Data.Builders {
    public interface IUntypedTableBuilder {
        void AddCell(IColumnRange columnRange, object value);
        void AddCells(IColumnRange columnRange, IEnumerable values);
        void NextRow();

        ITable Build();
    }
}