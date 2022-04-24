using N3O.Umbraco.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Extensions;

namespace N3O.Umbraco.Security {
    public class Authorization : IAuthorization {
        public AuthorizedResult IsAuthorized(IUser user, AccessControlList accessControlList) {
            var authResults = new List<AuthorizedResult>();
            
            authResults.Add(HasAccessAllowed(user));
            authResults.Add(HasRequiredPermissions(user, accessControlList));
            authResults.Add(MeetsRestrictions(user, accessControlList));

            var result = authResults.OrderBy(x => x.Authorized ? 1 : 0).First();

            return result;
        }

        private AuthorizedResult HasAccessAllowed(IUser user) {
            var accessAllowed = user == null || user.UserState == UserState.Active;

            if (accessAllowed) {
                return AuthorizedResult.IsAuthorized();
            } else {
                return AuthorizedResult.IsUnauthorized(UnauthorizedReasons.UserAccessDenied);
            }
        }

        private AuthorizedResult HasRequiredPermissions(IUser user, AccessControlList accessControlList) {
            var userPermissionIds = new List<string>();

            if (user == null) {
                userPermissionIds.AddRange(accessControlList.PermissionIds);
            } else {
                userPermissionIds.AddRange(user.Groups.SelectMany(x => x.Permissions));
            }

            var requiredPermissionIds = accessControlList.PermissionIds;
            var hasPermissions = !requiredPermissionIds.Any();

            if (requiredPermissionIds.Any()) {
                switch (accessControlList.AuthorizedIf) {
                    case AuthorizedIf.HasAll:
                        hasPermissions = userPermissionIds.ContainsAll(requiredPermissionIds);
                        break;

                    case AuthorizedIf.HasAny:
                        hasPermissions = userPermissionIds.ContainsAny(requiredPermissionIds);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            if (hasPermissions) {
                return AuthorizedResult.IsAuthorized();
            } else {
                return AuthorizedResult.IsUnauthorized(UnauthorizedReasons.InsufficientPermissions);
            }
        }

        private AuthorizedResult MeetsRestrictions(IUser user, AccessControlList accessControlList) {
            var meetsRestrictions = (accessControlList.RestrictedTo == null) || user == null;
            
            if (!meetsRestrictions) {
                switch (accessControlList.RestrictedTo.Value) {
                    case RestrictTo.Admin:
                        meetsRestrictions = user.IsAdmin();
                        break;

                    default:
                        throw UnrecognisedValueException.For(accessControlList.RestrictedTo.Value);
                }
            }

            if (meetsRestrictions) {
                return AuthorizedResult.IsAuthorized();
            } else {
                return AuthorizedResult.IsUnauthorized(UnauthorizedReasons.RestrictionsNotMet);
            }
        }
    }
}