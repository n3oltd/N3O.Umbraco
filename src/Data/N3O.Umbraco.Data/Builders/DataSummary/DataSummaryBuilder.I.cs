using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Builders; 

public interface IDataSummaryBuilder {
    DataSummary Build();
    IDataSummaryRowBuilder AddRow();
    IDataSummaryBuilder LinesAfter(int lines);
    IDataSummaryBuilder LinesBefore(int lines);
}