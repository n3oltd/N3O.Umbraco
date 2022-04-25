using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Data.Parsing {
    public interface ILookupParser : IDataTypeParser<INamedLookup> {
        ParseResult<TLookup> Parse<TLookup>(string text) where TLookup : class, INamedLookup;
        ParseResult<TLookup> Parse<TLookup>(JToken token) where TLookup : class, INamedLookup;
    }
}