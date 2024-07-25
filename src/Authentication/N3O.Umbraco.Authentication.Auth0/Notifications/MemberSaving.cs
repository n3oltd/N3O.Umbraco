using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Authentication.Auth0.Notifications;

public class MemberSaving : INotificationAsyncHandler<MemberSavingNotification> {
    private readonly Lazy<IUserDirectory> _userDirectory;
    private readonly Auth0MemberAuthenticationOptions _auth0MemberOptions;

    public MemberSaving(Lazy<IUserDirectory> userDirectory,
                        IOptions<Auth0MemberAuthenticationOptions> auth0MembersOptions) {
        _userDirectory = userDirectory;
        _auth0MemberOptions = auth0MembersOptions.Value;
    }

    public async Task HandleAsync(MemberSavingNotification notification, CancellationToken cancellationToken) {
        if (_auth0MemberOptions.Auth0.Login.AutoCreateDirectoryUser) {
            foreach (var user in notification.SavedEntities) {
                await _userDirectory.Value.CreateUserIfNotExistsAsync(ClientTypes.Members,
                                                                      _auth0MemberOptions.Auth0.Login.ClientId,
                                                                      _auth0MemberOptions.Auth0.Login.ConnectionName,
                                                                      user.Email,
                                                                      user.Name,
                                                                      lastName: null);
            }
        }
    }
}