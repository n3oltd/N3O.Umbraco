using System.Threading.Tasks;

namespace N3O.Umbraco.Elements;

public interface IElementsManager {
    Task SaveAndPublishDonationFormAsync();
    Task SaveAndPublishElementsSettingsAsync();
}