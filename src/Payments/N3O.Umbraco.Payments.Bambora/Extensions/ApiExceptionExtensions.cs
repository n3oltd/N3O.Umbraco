using Refit;
using System.Net;

namespace N3O.Umbraco.Payments.Bambora.Extensions;

public static class ApiExceptionExtensions {
    public static bool RequiresThreeDSecure(this ApiException apiException) {
        return apiException.StatusCode == HttpStatusCode.Found;
    }
}
