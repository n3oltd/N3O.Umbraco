using Humanizer.Bytes;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Parsing;

[Order(0)]
public class UrlBlobResolver : BlobResolver {
    protected override bool TryCanResolve(string value) {
        return value.IsValidUrl();
    }

    protected override async Task<Blob> TryResolveAsync(string url) {
        var client = new HttpClient();

        var response = await client.GetAsync(url);

        var filename = GetFilename(url, response.Content.Headers);
        var contentLength = ByteSize.FromBytes(response.Content.Headers.ContentLength.GetValueOrThrow());
        var contentType = GetContentType(filename, response.Content.Headers.ContentType);
        var stream = await response.Content.ReadAsStreamAsync();

        return new Blob(filename, null, contentType, contentLength, stream);
    }

    private string GetFilename(string url, HttpContentHeaders header) {
        var headerValues = header.ToDictionary();
        var content = headerValues["Content-Disposition"].FirstOrDefault();
        
        try {
            if (content.HasValue()) {
                content = content.Split(";").Last();
                content = content.Split("filename=").Last();
                content = content.Replace(" ", "");
                
                return content.Replace("\"", "");
            } else {
                return Path.GetFileName(url);
            }
        } catch {
            return "unknown";
        }
    }

    private string GetContentType(string filename, MediaTypeHeaderValue header) {
        if (header != null) {
            return header.MediaType;
        } else {
            return FileUtility.GetContentType(filename);
        }
    }
}
