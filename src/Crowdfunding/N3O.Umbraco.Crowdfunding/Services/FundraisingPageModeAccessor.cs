using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Members;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding; 

public class FundraisingPageModeAccessorAccessor : IFundraisingPageModeAccessor {
    private readonly IMembersAccessControl _membersAccessControl;

    public FundraisingPageModeAccessorAccessor(IMembersAccessControl membersAccessControl) {
        _membersAccessControl = membersAccessControl;
    }
    
    public FundraisingPageMode GetPageMode(Member member, IContent content) {
        if (_membersAccessControl.CanEdit(member, content)) {
            return FundraisingPageModes.Edit;
        } else {
            return FundraisingPageModes.View;
        }
    }
}