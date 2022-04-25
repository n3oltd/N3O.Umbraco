using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Parsing {
    public interface IContentParser : IDataTypeParser<IContent> {
        ParseResult<IContent> Parse(string text, Type targetType, Guid? parentId);
        ParseResult<IContent> Parse(JToken token, Type targetType, Guid? parentId);
    }
}