using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface INestedContentItemValidator {
    bool IsValidator(IPublishedElement content);
    void Validate(IPublishedElement content, IContent containerContent);
}
