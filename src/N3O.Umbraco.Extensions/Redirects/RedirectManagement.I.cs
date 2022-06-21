using System;

namespace N3O.Umbraco.Redirects;

public interface IRedirectManagement {
    Redirect FindRedirect(string requestPath);
    void LogHit(Guid redirectId);
}
