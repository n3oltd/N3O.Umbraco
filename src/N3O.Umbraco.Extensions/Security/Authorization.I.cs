using Umbraco.Cms.Core.Models.Membership;

namespace N3O.Umbraco.Security;

public interface IAuthorization {
    AuthorizedResult IsAuthorized(IUser user, AccessControlList accessControlList);
}
