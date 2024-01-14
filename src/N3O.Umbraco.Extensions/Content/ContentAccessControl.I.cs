using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content; 

public interface IContentAccessControl {
    bool CanApply(IContent content); // or maybe Task<bool>, is it for this type of content
    bool CanEdit(IMember member, IContent content); // Can this member edit this content
}