using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content {
    public interface IContentValidator {
        bool IsValidator(IContent content);
        void Validate(IContent content);
    }
}
