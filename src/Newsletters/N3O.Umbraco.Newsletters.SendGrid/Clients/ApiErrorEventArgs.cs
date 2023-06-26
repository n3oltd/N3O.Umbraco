using N3O.Umbraco.Newsletters.SendGrid.Lookups;
using System;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class ApiErrorEventArgs : EventArgs {
    public ApiErrorEventArgs(ApiError error) {
        Error = error;
    }

    public ApiError Error { get; }
}