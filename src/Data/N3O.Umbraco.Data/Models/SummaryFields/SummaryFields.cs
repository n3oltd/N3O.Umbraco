using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models; 

public class SummaryFields {
    public SummaryFields(int linesAfter, int linesBefore, IEnumerable<SummaryField> fields) {
        LinesAfter = linesAfter;
        LinesBefore = linesBefore;
        Fields = fields;
    }
    
    public int LinesAfter { get; }
    public int LinesBefore { get; }
    public IEnumerable<SummaryField> Fields { get; }
}