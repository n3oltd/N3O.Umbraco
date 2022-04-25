using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Parsing {
    public interface IPublishedContentParser : IDataTypeParser<IPublishedContent> {
        ParseResult<IPublishedContent> Parse(string text, Type targetType, Guid? parentId);
        ParseResult<IPublishedContent> Parse(JToken token, Type targetType, Guid? parentId);
    }
}