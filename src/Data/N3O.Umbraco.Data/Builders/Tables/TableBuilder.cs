namespace N3O.Umbraco.Data.Builders;

public class TableBuilder : ITableBuilder {
    private readonly IColumnRangeBuilder _columnRangeBuilder;

    public TableBuilder(IColumnRangeBuilder columnRangeBuilder) {
        _columnRangeBuilder = columnRangeBuilder;
    }

    public ITypedTableBuilder<TRow> Typed<TRow>(string name) {
        return new TypedTableBuilder<TRow>(_columnRangeBuilder, name);
    }

    public IUntypedTableBuilder Untyped(string name) {
        return new UntypedTableBuilder(name);
    }
}
