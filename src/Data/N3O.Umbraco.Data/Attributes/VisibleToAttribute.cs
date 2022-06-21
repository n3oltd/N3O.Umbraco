using N3O.Umbraco.Security;
using System;

namespace N3O.Umbraco.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class VisibleToAttribute : Attribute {
    public VisibleToAttribute() {
        AccessControlList = AccessControlList.AuthenticatedUsers();
    }

    public VisibleToAttribute(RestrictTo restrictedTo) {
        AccessControlList = AccessControlList.Restriction(restrictedTo);
    }

    public VisibleToAttribute(string permissionId) {
        AccessControlList = AccessControlList.RequirePermission(permissionId);
    }

    public VisibleToAttribute(RestrictTo restrictedTo, string permissionId)
        : this(restrictedTo, AuthorizedIf.HasAny, permissionId) { }

    public VisibleToAttribute(AuthorizedIf authorizedIf, params string[] permissionIds) {
        AccessControlList = AccessControlList.RequirePermissionIds(authorizedIf, permissionIds);
    }

    public VisibleToAttribute(RestrictTo restrictedTo, AuthorizedIf authorizedIf, params string[] permissionIds) {
        AccessControlList = new AccessControlList(restrictedTo, authorizedIf, permissionIds);
    }
    
    public AccessControlList AccessControlList { get; }
}
