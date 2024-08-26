namespace N3O.Umbraco.Engage.Security;

public interface IUserDirectoryIdAccessor {
    Task<string> GetIdAsync();
}
