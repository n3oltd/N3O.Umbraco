namespace N3O.Umbraco.OpenGraph;

public interface IOpenGraphBuilder {
    IOpenGraphBuilder WithTitle(string title);
    IOpenGraphBuilder WithDescription(string description);
    IOpenGraphBuilder WithImageUrl(string imageUrl);
    IOpenGraphBuilder WithRelativeImageUrl(string relativeImageUrl);
    IOpenGraphBuilder WithUrl(string url);

    OpenGraphData Build();
    
    bool HasData { get; }
}