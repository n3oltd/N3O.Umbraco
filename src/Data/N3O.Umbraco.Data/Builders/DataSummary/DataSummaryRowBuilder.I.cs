using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Builders; 

public interface IDataSummaryRowBuilder {
    DataSummaryRow Build(IFormatter formatter);
    IDataSummaryRowBuilder AddDateTime(DateTime? value);
    IDataSummaryRowBuilder AddDecimal(Decimal value);
    IDataSummaryRowBuilder AddInt(int value);
    IDataSummaryRowBuilder AddPercentage(decimal value);
    IDataSummaryRowBuilder AddText(string value);
    IDataSummaryRowBuilder WithLabel(string label);
    IDataSummaryRowBuilder WithLabel<T>(Func<T, string> getText) where T : class, IStrings, new();
}