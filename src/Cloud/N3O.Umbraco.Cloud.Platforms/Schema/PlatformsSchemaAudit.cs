using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

// GetValue returns default when a property is absent, so a content class bound to a type that does not exist
// reads as empty forever rather than throwing. Every alias the framework binds is checked here so that
// failure is loud once, at boot, instead of silent in every request.
// The check stops at the content type. Whether a given class property is read out of the content or computed
// in its getter is not visible to reflection, and the two are otherwise identical, so a property level check
// here cannot tell a genuinely missing property from a computed one. The seeder declares every property it
// owns, which makes it the authority on that question
public class PlatformsSchemaAudit : IPlatformsSchemaAudit {
    private readonly IContentTypeService _contentTypeService;

    public PlatformsSchemaAudit(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }

    public IReadOnlyList<string> FindGaps() {
        var gaps = new List<string>();

        foreach (var contentClass in GetContentClasses()) {
            var alias = AliasHelper.ContentTypeAlias(contentClass);

            if (_contentTypeService.Get(alias) == null) {
                gaps.Add($"{contentClass.Name} binds content type {alias.Quote()} which does not exist");
            }
        }

        return gaps;
    }

    private IEnumerable<Type> GetContentClasses() {
        return typeof(PlatformsSchemaAudit).Assembly
                                           .GetTypes()
                                           .Where(x => x.GetCustomAttribute<UmbracoContentAttribute>() != null)
                                           .OrderBy(x => x.Name);
    }
}
