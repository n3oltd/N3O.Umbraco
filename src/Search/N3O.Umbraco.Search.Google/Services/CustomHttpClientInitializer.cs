using Google.Apis.Http;

namespace N3O.Umbraco.Search.Google;

public class CustomHttpClientInitializer : IConfigurableHttpClientInitializer {
    private readonly string _referer;

    public CustomHttpClientInitializer(string referer) {
        _referer = referer;
    }

    public void Initialize(ConfigurableHttpClient httpClient) {
        if (!string.IsNullOrEmpty(_referer)) {
            httpClient.DefaultRequestHeaders.Add("Referer", _referer);
        }
    }
}
