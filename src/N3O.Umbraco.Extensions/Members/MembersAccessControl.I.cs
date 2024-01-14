using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Members; 

public interface IMembersAccessControl {
    bool CanEdit(IMember member, IContent content);
    Task<bool> CanEditAsync(Guid pageId);
    Task<bool> CanEditAsync(IContent content);
}