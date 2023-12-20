using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Builders;

public class FluentSummaryFieldsBuilder : IFluentSummaryFieldsBuilder {
    private readonly List<ISummaryFieldBuilder> _fieldBuilders = new();
    private int _linesBefore;
    private int _linesAfter;

    public ISummaryFieldBuilder AddField() {
        var fieldBuilder = new SummaryFieldBuilder();
        
        _fieldBuilders.Add(fieldBuilder);
        
        return fieldBuilder;
    }

    public IFluentSummaryFieldsBuilder SetLinesAfter(int lines) {
        _linesAfter = lines;
        
        return this;
    }
    
    public IFluentSummaryFieldsBuilder SetLinesBefore(int lines) {
        _linesBefore = lines;
        
        return this;
    }

    public SummaryFields Build(IFormatter formatter) {
        Validate();
        
        var fields = _fieldBuilders.Select(x => x.Build(formatter)).ToList();

        return new SummaryFields(_linesAfter, _linesBefore, fields);
    }

    private void Validate() {
        if (_fieldBuilders.None()) {
            throw new Exception("At least one field must be specified");
        }
    }
}