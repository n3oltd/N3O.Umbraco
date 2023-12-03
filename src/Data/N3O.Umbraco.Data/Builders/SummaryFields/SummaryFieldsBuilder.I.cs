using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Builders; 

public interface ISummaryFieldsBuilder {
    ISummaryFieldBuilder AddField();
    ISummaryFieldsBuilder SetLinesAfter(int lines);
    ISummaryFieldsBuilder SetLinesBefore(int lines);
    SummaryFields Build(IFormatter formatter);
}