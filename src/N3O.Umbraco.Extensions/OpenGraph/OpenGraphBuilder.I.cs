namespace N3O.Umbraco.OpenGraph;

public interface IOpenGraphBuilder {
    IOpenGraphBuilder WithTitle(string title);
    IOpenGraphBuilder WithDescription(string description);
    IOpenGraphBuilder WithRelativeImageUrl(string relativeImageUrl);
    IOpenGraphBuilder WithImageUrl(string imageUrl);
    IOpenGraphBuilder WithUrl(string url);

    OpenGraphData Build();
    
    bool HasData { get; }
}