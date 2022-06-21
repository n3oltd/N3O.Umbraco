using N3O.Umbraco.Data.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders;

public interface ITypedTableBuilder<TRow> {
    void AddRow(TRow row, Action<AddedRow<TRow>> rowAddedCallback = null);
    void AddRows(IEnumerable<TRow> rows, Action<AddedRow<TRow>> rowAddedCallback = null);

    ITable Build();
}
