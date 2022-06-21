using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public class BlobParser : DataTypeParser<Blob>, IBlobParser {
    private readonly IReadOnlyList<IBlobResolver> _blobResolvers;

    public BlobParser(IEnumerable<IBlobResolver> blobResolvers) {
        _blobResolvers = blobResolvers.OrEmpty().ApplyAttributeOrdering();
    }
    
    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.Blob;
    }

    protected override ParseResult<Blob> TryParse(string text, Type targetType) {
        Blob blob = null;
        
        if (text.HasValue()) {
            var blobResolver = _blobResolvers.FirstOrDefault(x => x.CanResolve(text));

            if (blobResolver != null) {
                var resolveResult = blobResolver.ResolveAsync(text).GetAwaiter().GetResult();

                if (resolveResult.Success) {
                    blob = resolveResult.Value;
                }
            }

            if (blob == null) {
                return ParseResult.Fail<Blob>();
            }
        }
        
        return ParseResult.Success<Blob>(blob);
    }

    protected override IEnumerable<JTokenType> TokenTypes => JTokenType.Uri.Yield();

    protected override ParseResult<Blob> TryParseToken(JToken token, Type targetType) {
        var uri = (Uri) token;

        return TryParse(uri?.ToString(), targetType);
    }
}
