using N3O.Umbraco.Data.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Services;

public interface ICsvReader : IDisposable {
    IReadOnlyList<string> GetColumnHeadings();
    bool ReadRow();
    ITable ReadTable(string name);
    void Skip(int rows);

    int ColumnCount { get; }
    CsvRow Row { get; }

    event EventHandler<CsvErrorEventArgs> OnRowError;
}
