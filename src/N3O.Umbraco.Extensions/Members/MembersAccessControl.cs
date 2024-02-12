using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Members;

public class MembersAccessControl : IMembersAccessControl {
    private readonly IContentService _contentService;
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IReadOnlyList<IContentAccessControl> _contentAccessControls;

    public MembersAccessControl(IContentService contentService,
                                IMemberManager memberManager,
                                IMemberService memberService,
                                IEnumerable<IContentAccessControl> contentAccessControls) {
        _contentService = contentService;
        _memberManager = memberManager;
        _memberService = memberService;
        _contentAccessControls = contentAccessControls.ToList();
    }

    public async Task<bool> CanEditAsync(Guid pageId) {
        var content = _contentService.GetById(pageId);

        return await CanEditAsync(content);
    }

    public async Task<bool> CanEditAsync(IContent content) {
        var memberIdentity = await _memberManager.GetCurrentMemberAsync();

        if (!memberIdentity.HasValue() || !content.HasValue()) {
            throw new Exception("Invalid input");
        }

        var member = _memberService.GetByKey(memberIdentity.Key);

        return CanEdit(content, member);
    }

    private bool CanEdit(IContent content, IMember member) {
        var accessController = _contentAccessControls.SingleOrDefault(x => x.CanApply(content));

        if (!accessController.HasValue()) {
            throw new Exception($"No access controller found for content type : {content.ContentType.Alias}");
        }

        return accessController.CanEdit(member, content);
    }
}