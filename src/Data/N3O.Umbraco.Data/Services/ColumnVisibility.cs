using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Security;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Data.Services {
    public class ColumnVisibility : IColumnVisibility {
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
        private readonly IAuthorization _authorization;

        public ColumnVisibility(IBackOfficeSecurityAccessor backOfficeSecurityAccessor, IAuthorization authorization) {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _authorization = authorization;
        }

        public bool IsVisible(Column column) {
            var isVisible = !column.Hidden;

            if (isVisible) {
                var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;

                var authorizedResult = _authorization.IsAuthorized(currentUser, column.AccessControlList);

                isVisible = authorizedResult.Authorized;
            }

            return isVisible;
        }
    }
}