using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Security {
    public class AccessControlList {
        public AccessControlList(RestrictTo? restrictedTo, AuthorizedIf? authorizedIf, params string[] permissionIds) {
            RestrictedTo = restrictedTo;
            AuthorizedIf = authorizedIf ?? AuthorizedIf.HasAll;
            PermissionIds = permissionIds.ToList();
        }

        public static AccessControlList AuthenticatedUsers() {
            return new AccessControlList(null, null);
        }

        public static AccessControlList AdminOnly(params string[] permissionIds) {
            return AdminOnly(AuthorizedIf.HasAny, permissionIds);
        }

        public static AccessControlList AdminOnly(AuthorizedIf authorizedIf, params string[] permissionIds) {
            return new AccessControlList(RestrictTo.Admin, authorizedIf, permissionIds);
        }

        public static AccessControlList Restriction(RestrictTo restrictedTo) {
            return new AccessControlList(restrictedTo, null);
        }

        public static AccessControlList RequirePermission(string permission) {
            return new AccessControlList(null, null, permission);
        }

        public static AccessControlList RequirePermissionIds(AuthorizedIf authorizedIf, params string[] permissionIds) {
            return new AccessControlList(null, authorizedIf, permissionIds);
        }

        public static AccessControlList RequireAllPermissionIds(params string[] permissionIds) {
            return new AccessControlList(null, AuthorizedIf.HasAll, permissionIds);
        }

        public static AccessControlList RequireAnyPermissionIds(params string[] permissionIds) {
            return new AccessControlList(null, AuthorizedIf.HasAny, permissionIds);
        }
        
        public AuthorizedIf AuthorizedIf { get; }
        public IReadOnlyList<string> PermissionIds { get; }
        public RestrictTo? RestrictedTo { get; }
    }
}