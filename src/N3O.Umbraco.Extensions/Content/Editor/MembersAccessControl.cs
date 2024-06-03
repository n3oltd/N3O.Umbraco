using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Content;

public abstract class MembersAccessControl : ContentAccessControl {
    private readonly IMemberManager _memberManager;
    private readonly IContentHelper _contentHelper;

    protected MembersAccessControl(IContentHelper contentHelper, IMemberManager memberManager)
        : base(contentHelper) {
        _memberManager = memberManager;
        _contentHelper = contentHelper;
    }

    protected override bool IsAccessControlFor(string contentTypeAlias) {
        return contentTypeAlias.EqualsInvariant(ContentTypeAlias);
    }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        return await AllowEditAsync(() => _contentHelper.GetDataListValues<IPublishedContent>(contentProperties,
                                                                                              PropertyAlias));
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(PropertyAlias));

        return await AllowEditAsync(() => (IEnumerable<IPublishedContent>) property?.GetValue(PropertyAlias));
    }
    
    private async Task<bool> AllowEditAsync(Func<IEnumerable<IPublishedContent>> getAllowedMembers) {
        var memberIdentity = await _memberManager.GetCurrentMemberAsync();
        
        if (memberIdentity == null) {
            return false;
        }

        var member = _memberManager.AsPublishedMember(memberIdentity);

        if (member == null) {
            return false;
        }

        var allowedMembers = getAllowedMembers().OrEmpty();

        return allowedMembers.Any(x => x.Key == member.Key);
    }
    
    protected abstract string ContentTypeAlias { get; }
    protected abstract string PropertyAlias { get; }
}