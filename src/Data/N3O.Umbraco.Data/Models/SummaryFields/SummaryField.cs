using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models; 

public class SummaryField {
    public SummaryField(string label, IEnumerable<Cell> cells) {
        Label = label;
        Cells = cells;
    }

    public string Label { get; }
    public IEnumerable<Cell> Cells { get; }
}