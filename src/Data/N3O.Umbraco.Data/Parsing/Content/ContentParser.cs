using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using DataType = N3O.Umbraco.Data.Lookups.DataType;

namespace N3O.Umbraco.Data.Parsing {
    public class ContentParser : DataTypeParser<IContent>, IContentParser {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IContentService _contentService;
        private readonly IContentHelper _contentHelper;

        public ContentParser(IUmbracoContextFactory umbracoContextFactory,
                             IContentService contentService,
                             IContentHelper contentHelper) {
            _umbracoContextFactory = umbracoContextFactory;
            _contentService = contentService;
            _contentHelper = contentHelper;
        }
        
        public override bool CanParse(DataType dataType) {
            return dataType == DataTypes.Content;
        }

        public override ParseResult<IContent> Parse(string text, Type targetType) {
            return Parse(text, targetType, null);
        }

        public ParseResult<IContent> Parse(string text, Type targetType, Guid? parentId) {
            IContent value = null;
            
            if (text.HasValue()) {
                using (_umbracoContextFactory.EnsureUmbracoContext()) {
                    text = text.Trim();
                    
                    if (Guid.TryParse(text, out var id)) {
                        value = _contentService.GetById(id);
                    } else {
                        var searchRoots = new List<IContent>();

                        if (parentId != null) {
                            searchRoots.Add(_contentService.GetById(parentId.Value));
                        } else {
                            searchRoots.AddRange(_contentService.GetRootContent());
                        }

                        var matches = searchRoots.SelectMany(r => _contentHelper.GetDescendants(r)
                                                                                .Where(c => c.Name.EqualsInvariant(text)))
                                                 .ToList();

                        if (matches.IsSingle()) {
                            value = matches.Single();
                        }
                    }

                    if (value == null) {
                        return ParseResult.Fail<IContent>();
                    }
                }
            }
            
            return ParseResult.Success(value);
        }

        public ParseResult<IContent> Parse(JToken token, Type targetType, Guid? parentId) {
            if (token.Type == JTokenType.String) {
                return Parse((string) token, targetType, parentId);
            } else {
                return ParseResult.Fail<IContent>();
            }
        }
    }
}