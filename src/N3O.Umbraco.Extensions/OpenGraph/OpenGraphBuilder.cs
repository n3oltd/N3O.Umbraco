using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.OpenGraph;

public class OpenGraphBuilder : IOpenGraphBuilder {
    private readonly IUrlBuilder _urlBuilder;
    private string _title;
    private string _imageUrl;
    private string _description;
    private string _url;

    public OpenGraphBuilder(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public IOpenGraphBuilder WithTitle(string title) {
        _title = title;

        return this;
    }

    public IOpenGraphBuilder WithDescription(string description) {
        _description = description;

        return this;
    }

    public IOpenGraphBuilder WithImagePath(string imagePath) {
        var imageUrl = _urlBuilder.Root().AppendPathSegment(imagePath);

        return WithImageUrl(imageUrl);
    }

    public IOpenGraphBuilder WithImageUrl(string imageUrl) {
        _imageUrl = imageUrl;

        return this;
    }

    public IOpenGraphBuilder WithUrl(string url) {
        _url = url;

        return this;
    }

    public OpenGraphData Build() {
        return new OpenGraphData(_title, _description, _url, _imageUrl);
    }

    public bool HasData => _title.HasValue() || _description.HasValue() || _imageUrl.HasValue();
}
