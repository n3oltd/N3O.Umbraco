using System.Threading.Tasks;

namespace N3O.Umbraco.Crm.Engage;

public interface IUserDirectoryIdAccessor {
    Task<string> GetIdAsync();
}
