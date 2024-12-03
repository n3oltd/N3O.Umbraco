using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface IElementsManager {
    Task CreateOrUpdateDonationOptionAsync();
}