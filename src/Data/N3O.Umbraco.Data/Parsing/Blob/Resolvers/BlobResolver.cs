using HeyRed.Mime;
using N3O.Umbraco.Data.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Parsing {
    public abstract class BlobResolver : IBlobResolver {
        public bool CanResolve(string value) {
            try {
                if (value == null) {
                    return false;
                }

                return TryCanResolve(value.Trim());
            } catch {
                return false;
            }
        }

        public async Task<ParseResult<Blob>> ResolveAsync(string value) {
            try {
                var blob = await TryResolveAsync(value.Trim());

                return ParseResult.Success(blob);
            } catch {
                return ParseResult.Fail<Blob>();
            }
        }

        protected string GetContentType(string filename) {
            try {
                return MimeTypesMap.GetMimeType(filename);
            } catch {
                return "application/octet-stream";
            }
        }

        protected abstract bool TryCanResolve(string value);
        protected abstract Task<Blob> TryResolveAsync(string value);
    }
}