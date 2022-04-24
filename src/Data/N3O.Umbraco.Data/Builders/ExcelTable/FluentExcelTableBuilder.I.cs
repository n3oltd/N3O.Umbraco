using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Builders {
    public interface IFluentExcelTableBuilder {
        void FormatColumn<TExcelCellConverter>(Column column);
        void Footer(Column column, AggregationFunction footerFunction);

        IExcelTable Build();
    }
}