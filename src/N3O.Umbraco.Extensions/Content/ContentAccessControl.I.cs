using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content; 

public interface IContentAccessControl {
    bool CanApply(IContent content);
    bool CanEdit(IMember member, IContent content);
}