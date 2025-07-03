using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders; 

public class SummaryFieldBuilder : ISummaryFieldBuilder {
    private readonly List<Cell> _cells = [];
    private Func<IFormatter, string> _getLabel;

    public ISummaryFieldBuilder AddCell(Cell cell) {
        _cells.Add(cell);

        return this;
    }

    public ISummaryFieldBuilder WithLabel(string label) {
        _getLabel = _=> label;
        
        return this;
    }

    public ISummaryFieldBuilder WithLabel<T>(Func<T, string> getText) where T : class, IStrings, new() {
        _getLabel = f => f.Text.Format(getText);
        
        return this;
    }
    
    public SummaryField Build(IFormatter formatter) {
        Validate();
        
        var label = _getLabel(formatter);
        
        return new SummaryField(label, _cells);
    }

    private void Validate() {
        if (_getLabel == null) {
            throw new Exception("Label must be specified");
        }

        if (_cells.None()) {
            throw new Exception("At least one cell must be specified");
        }
    }
}