using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
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
            foreach (var member in notification.SavedEntities) {
                var auth0User = await _userDirectory.Value.CreateUserIfNotExistsAsync(ClientTypes.Members,
                                                                      _auth0MemberOptions.Auth0.Login.ClientId,
                                                                      _auth0MemberOptions.Auth0.Login.ConnectionName,
                                                                      member.Email,
                                                                      member.Name,
                                                                      lastName: null);

                if (auth0User.HasValue()) {
                    SetMemberAvatarLink(member, auth0User.Picture);
                }
            }
        }
    }

    private void SetMemberAvatarLink(IMember member, string pictureUrl) {
        if (member.HasProperty(MemberConstants.Member.Properties.AvatarLink) &&
            !member.GetValue<string>(MemberConstants.Member.Properties.AvatarLink).Equals(pictureUrl)) {
            member.SetValue(MemberConstants.Member.Properties.AvatarLink, pictureUrl);
        }
    }
}