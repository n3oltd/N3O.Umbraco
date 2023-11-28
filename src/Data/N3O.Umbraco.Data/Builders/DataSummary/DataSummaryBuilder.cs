using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Builders; 

public class DataSummaryBuilder : IDataSummaryBuilder {
    private readonly List<IDataSummaryRowBuilder> _rowBuilders = new ();
    private int _linesBefore;
    private int _linesAfter;

    public IDataSummaryRowBuilder AddRow() {
        var rowBuilder = new DataSummaryRowBuilder();
        _rowBuilders.Add(rowBuilder);
        return rowBuilder;
    }

    public IDataSummaryBuilder LinesBefore(int lines) {
        _linesBefore = lines;
        return this;
    }
    
    public IDataSummaryBuilder LinesAfter(int lines) {
        _linesAfter = lines;
        return this;
    }

    public DataSummary Build() {
        var rows = _rowBuilders.Select(x => x.Build(Formatter.Default))
                               .ToList();

        return new DataSummary(_linesBefore, _linesAfter, rows);
    }
}