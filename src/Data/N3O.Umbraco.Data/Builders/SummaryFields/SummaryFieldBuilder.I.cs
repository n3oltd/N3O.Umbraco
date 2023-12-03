using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Builders; 

public interface ISummaryFieldBuilder {
    ISummaryFieldBuilder AddCell(Cell cell);
    ISummaryFieldBuilder WithLabel(string label);
    ISummaryFieldBuilder WithLabel<T>(Func<T, string> getText) where T : class, IStrings, new();
    
    SummaryField Build(IFormatter formatter);
}