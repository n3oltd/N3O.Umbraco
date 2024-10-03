using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.OpenGraph;

public class OpenGraphBuilder : IOpenGraphBuilder {
    private string _title;
    private string _imageUrl;
    private string _description;

    public IOpenGraphBuilder WithTitle(string title) {
        _title = title;

        return this;
    }

    public IOpenGraphBuilder WithDescription(string description) {
        _description = description;

        return this;
    }

    public IOpenGraphBuilder WithImageUrl(string imageUrl) {
        _imageUrl = imageUrl;

        return this;
    }

    public OpenGraphData Build() {
        return new OpenGraphData(_title, _description, _imageUrl);
    }

    public bool HasData => _title.HasValue() || _description.HasValue() || _imageUrl.HasValue();
}
