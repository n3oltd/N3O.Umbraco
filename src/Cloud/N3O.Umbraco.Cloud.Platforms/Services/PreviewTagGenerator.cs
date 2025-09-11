using MimeKit.Text;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class PreviewTagGenerator : IPreviewTagGenerator {
    private readonly ICdnClient _cdnClient;
    private readonly IJsonProvider _jsonProvider;

    protected PreviewTagGenerator(ICdnClient cdnClient, IJsonProvider jsonProvider) {
        _cdnClient = cdnClient;
        _jsonProvider = jsonProvider;
    }
    
    public bool CanGeneratePreview(string contentTypeAlias) {
        return contentTypeAlias.EqualsInvariant(ContentTypeAlias);
    }
    
    public async Task<(string ETag, string Html)> GeneratePreviewTagAsync(IReadOnlyDictionary<string, object> content) {
        var json = await GeneratePreviewJsonAsync(content);

        var etag = json.Sha1();
        var html = $"<n3o-donation-form-modal form-id='{Guid.NewGuid()}' preview='true' json='{HtmlUtils.HtmlEncode(json)}'></n3o-donation-form-modal>";
        
        return (etag, html);
    }
    
    protected abstract string ContentTypeAlias { get; }
    
    protected abstract void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData);

    private async Task<string> GeneratePreviewJsonAsync(IReadOnlyDictionary<string, object> content) {
        var previewData = new Dictionary<string, object>();

        previewData["FundStructure"] = await _cdnClient.DownloadSubscriptionContentAsync<PublishedFundStructure>(SubscriptionFiles.FundStructure,
                                                                                                           JsonSerializers.JsonProvider);
        
        previewData["Currencies"] = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCurrencies>(SubscriptionFiles.Currencies, 
                                                                                                           JsonSerializers.Simple);

        PopulatePreviewData(content, previewData);

        return _jsonProvider.SerializeObject(previewData);
    }
}