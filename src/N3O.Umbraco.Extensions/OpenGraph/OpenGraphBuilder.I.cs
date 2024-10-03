namespace N3O.Umbraco.OpenGraph;

public interface IOpenGraphBuilder {
    IOpenGraphBuilder WithTitle(string title);
    IOpenGraphBuilder WithDescription(string description);
    IOpenGraphBuilder WithImagePath(string imagePath);
    IOpenGraphBuilder WithImageUrl(string imageUrl);

    OpenGraphData Build();
    
    bool HasData { get; }
}