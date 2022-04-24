using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Exceptions {
    public class ColumnNotFoundException : Exception {
        public ColumnNotFoundException(string columnName)
            : base($"No column found with name {$"{columnName}".Quote()}") { }

        public ColumnNotFoundException(int index) : base($"No column found with index {index}") { }
    }
}