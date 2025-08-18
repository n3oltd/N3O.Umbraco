using System.Threading.Tasks;

namespace N3O.Umbraco.Helpdesk.Services;

public interface IHelpdeskIssueManager {
    Task CreateIssueAsync();
    
}