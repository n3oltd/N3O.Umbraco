using System.Threading.Tasks;

namespace N3O.Umbraco.Elements;

public interface IElementsManager {
    Task SaveAndPublishCheckoutProfileAsync();
    Task SaveAndPublishDonationFormAsync();
    Task SaveAndPublishElementsSettingsAsync();
}