using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Builders;

public interface IFluentSummaryFieldsBuilder {
    ISummaryFieldBuilder AddField();
    IFluentSummaryFieldsBuilder SetLinesAfter(int lines);
    IFluentSummaryFieldsBuilder SetLinesBefore(int lines);
    SummaryFields Build(IFormatter formatter);
}