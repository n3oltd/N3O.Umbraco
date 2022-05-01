using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using OurDataType = N3O.Umbraco.Data.Lookups.DataType;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing {
    public class PublishedContentParser : DataTypeParser<IPublishedContent>, IPublishedContentParser {
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public PublishedContentParser(IUmbracoContextFactory umbracoContextFactory) {
            _umbracoContextFactory = umbracoContextFactory;
        }
        
        public override bool CanParse(OurDataType dataType) {
            return dataType == OurDataTypes.Content;
        }

        public override ParseResult<IPublishedContent> Parse(string text, Type targetType) {
            return Parse(text, targetType, null);
        }

        public ParseResult<IPublishedContent> Parse(string text, Type targetType, Guid? parentId) {
            IPublishedContent value = null;
            
            if (text.HasValue()) {
                using (var contextReference = _umbracoContextFactory.EnsureUmbracoContext()) {
                    text = text.Trim();
                    
                    if (Guid.TryParse(text, out var id)) {
                        value = contextReference.UmbracoContext.Content.GetById(id);
                    } else {
                        var searchRoots = new List<IPublishedContent>();

                        if (parentId != null) {
                            searchRoots.Add(contextReference.UmbracoContext.Content.GetById(parentId.Value));
                        } else {
                            searchRoots.AddRange(contextReference.UmbracoContext.Content.GetAtRoot());
                        }

                        var matches = searchRoots.SelectMany(r => r.Descendants()
                                                                   .Where(c => c.Name.EqualsInvariant(text)))
                                                 .ToList();

                        if (matches.IsSingle()) {
                            value = matches.Single();
                        }
                    }

                    if (value == null) {
                        return ParseResult.Fail<IPublishedContent>();
                    }
                }
            }
            
            return ParseResult.Success(value);
        }

        public ParseResult<IPublishedContent> Parse(JToken token, Type targetType, Guid? parentId) {
            if (token.Type == JTokenType.String) {
                return Parse((string) token, targetType, parentId);
            } else {
                return ParseResult.Fail<IPublishedContent>();
            }
        }
    }
}