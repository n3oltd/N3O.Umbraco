using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders; 

public class DataSummaryRowBuilder : IDataSummaryRowBuilder {
    private Func<IFormatter,string> _getLabel;
    private List<DataSummaryRowValue> _rowValues = new();

    public DataSummaryRow Build(IFormatter formatter) {
        var label = _getLabel(formatter);
        return new DataSummaryRow(label, _rowValues);
    }

    public IDataSummaryRowBuilder WithLabel(string label) {
        _getLabel =_=> label;
        return this;
    }

    public IDataSummaryRowBuilder WithLabel<T>(Func<T, string> getText) where T : class, IStrings, new() {
        _getLabel = f => f.Text.Format(getText);
        return this;
    }

    public IDataSummaryRowBuilder AddDateTime(DateTime? value) {
        return AddValue(value, new DateTimeExcelNumberFormat(DateFormats.DayMonthYearSlashes, TimeFormats._12));
    }

    public IDataSummaryRowBuilder AddDecimal(decimal value) {
        return AddValue(value, new DecimalExcelNumberFormat());
    }

    public IDataSummaryRowBuilder AddInt(int value) {
        return AddValue(value, new IntegerExcelNumberFormat());
    }

    public IDataSummaryRowBuilder AddText(string value) {
        return AddValue(value, new StringExcelNumberFormat());
    }
    
    public IDataSummaryRowBuilder AddPercentage(decimal value) {
        return AddValue(value, new BoolExcelNumberFormat());
    }
    
    private IDataSummaryRowBuilder AddValue<T>(T value, ExcelNumberFormat formatting) {
        var rowValue = new DataSummaryRowValue(value, formatting);
        _rowValues.Add(rowValue);
        return this;
    }
}