using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Builders; 

public interface IDataSummaryBuilder {
    DataSummary Build();
    IDataSummaryRowBuilder AddRow();
    IDataSummaryBuilder LinesBefore(int lines);
    IDataSummaryBuilder LinesAfter(int lines);
}