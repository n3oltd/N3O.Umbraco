using N3O.Umbraco.Crowdfunding.Lookups;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding; 

public interface IFundraisingPageModeAccessor {
    Task<FundraisingPageMode> GetPageMode(Guid pageId);
}

