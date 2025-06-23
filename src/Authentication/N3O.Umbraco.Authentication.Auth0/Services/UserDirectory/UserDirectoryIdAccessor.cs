﻿using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.NamedParameters;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectoryIdAccessor : IUserDirectoryIdAccessor {
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private readonly Lazy<IMemberManager> _memberManager;
    private readonly Lazy<IMemberService> _memberService;
    private readonly Lazy<IBackOfficeSecurityAccessor> _backOfficeSecurityAccessor;
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IUserDirectory _userDirectory;
    private readonly UserDirectoryId _userDirectoryId;
    private string _value;

    public UserDirectoryIdAccessor(Lazy<IMemberManager> memberManager,
                                   Lazy<IMemberService> memberService,
                                   Lazy<IBackOfficeSecurityAccessor> backOfficeSecurityAccessor,
                                   IUserDirectory userDirectory,
                                   IUmbracoContextFactory umbracoContextFactory,
                                   UserDirectoryId userDirectoryId = null) {
        _memberManager = memberManager;
        _userDirectory = userDirectory;
        _memberService = memberService;
        _umbracoContextFactory = umbracoContextFactory;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _userDirectoryId = userDirectoryId;
    }

    public async Task<string> GetIdAsync(UmbracoAuthType umbracoAuthType) {
        if (_value == null) {
            _value ??= _userDirectoryId?.Value;

            if (_value == null) {
                using (_umbracoContextFactory.EnsureUmbracoContext()) {
                    var email = await GetEmailAsync(umbracoAuthType);

                    if (email.HasValue()) {
                        _value = await Cache.GetOrAddAtomicAsync(email, async () => {
                            var directoryUser = await _userDirectory.GetUserByEmailAsync(umbracoAuthType, email);

                            return directoryUser?.UserId;
                        });
                    }
                }
            }
        }

        return _value;
    }

    private async Task<string> GetEmailAsync(UmbracoAuthType umbracoAuthType) {
        if (umbracoAuthType == UmbracoAuthTypes.User) {
            return GetUserEmail();
        } else if (umbracoAuthType == UmbracoAuthTypes.Member) {
            return await GetMemberEmailAsync();
        } else {
            throw UnrecognisedValueException.For(umbracoAuthType);
        }
    }

    private async Task<string> GetMemberEmailAsync() {
        var publishedMember = await _memberManager.Value.GetCurrentPublishedMemberAsync();
        var member = publishedMember.IfNotNull(x => _memberService.Value.GetById(x.Id));

        return member?.Email;
    }
    
    private string GetUserEmail() {
        var currentUser = _backOfficeSecurityAccessor.Value.BackOfficeSecurity?.CurrentUser;

        return currentUser?.Email;
    }
}