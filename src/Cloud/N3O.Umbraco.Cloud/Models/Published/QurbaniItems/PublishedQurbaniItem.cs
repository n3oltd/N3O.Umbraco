namespace N3O.Umbraco.Cloud.Models;

public class PublishedQurbaniItem : PublishedNamedLookup {
    public PublishedFundDimensionValues FundDimensionValues { get; set; }
    public PublishedPrice Price { get; set; }
}