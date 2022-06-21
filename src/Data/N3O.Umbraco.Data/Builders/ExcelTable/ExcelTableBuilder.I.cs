using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Builders;

public interface IExcelTableBuilder {
    IFluentExcelTableBuilder ForTable(ITable table);
}
