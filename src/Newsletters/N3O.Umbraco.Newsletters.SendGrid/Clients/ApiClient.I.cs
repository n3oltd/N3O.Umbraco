using System;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public interface IApiClient {
    event EventHandler<ApiErrorEventArgs> ApiError;
}