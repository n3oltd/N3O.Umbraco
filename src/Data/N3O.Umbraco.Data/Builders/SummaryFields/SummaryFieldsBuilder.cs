namespace N3O.Umbraco.Data.Builders; 

public class SummaryFieldsBuilder : ISummaryFieldsBuilder {
    public IFluentSummaryFieldsBuilder Create() {
        return new FluentSummaryFieldsBuilder();
    }
}