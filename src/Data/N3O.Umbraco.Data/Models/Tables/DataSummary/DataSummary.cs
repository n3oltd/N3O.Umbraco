using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models; 

public class DataSummary {
    public DataSummary(int linesBefore, int linesAfter, IEnumerable<DataSummaryRow> rows) {
        LinesBefore = linesBefore;
        LinesAfter = linesAfter;
        Rows = rows;
    }
    
    public int LinesBefore { get; }
    public int LinesAfter { get; }
    public IEnumerable<DataSummaryRow> Rows { get; }
}