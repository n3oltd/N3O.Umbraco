namespace N3O.Umbraco.Data.Models;

public class CellAddress : Value {
    public CellAddress(Column column, int row) {
        Column = column;
        Row = row;
    }

    public Column Column { get; }
    public int Row { get; }
}
