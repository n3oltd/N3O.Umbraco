namespace N3O.Umbraco.Data.Builders;

public interface ITableBuilder {
    ITypedTableBuilder<TRow> Typed<TRow>(string name);
    IUntypedTableBuilder Untyped(string name);
}
