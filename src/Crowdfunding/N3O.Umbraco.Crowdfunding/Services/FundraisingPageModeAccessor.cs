using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Members;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding; 

public class FundraisingPageModeAccessor : IFundraisingPageModeAccessor {
    private readonly IMembersAccessControl _membersAccessControl;

    public FundraisingPageModeAccessor(IMembersAccessControl membersAccessControl) {
        _membersAccessControl = membersAccessControl;
    }
    
    public async Task<FundraisingPageMode> GetPageMode(Guid pageId) {
        if (await _membersAccessControl.CanEditAsync(pageId)) {
            return FundraisingPageModes.Edit;
        } else {
            return FundraisingPageModes.View;
        }
    }
}