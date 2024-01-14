using N3O.Umbraco.Crowdfunding.Lookups;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding; 

public interface IFundraisingPageModeAccessor {
    FundraisingPageMode GetPageMode(Member member, IContent content);
}

