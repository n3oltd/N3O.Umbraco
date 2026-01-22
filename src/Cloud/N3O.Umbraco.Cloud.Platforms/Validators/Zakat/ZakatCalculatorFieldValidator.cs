using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ZakatCalculatorFieldValidator : ContentValidator {
    private static readonly string MetalPropertyAlias = AliasHelper<ZakatCalculatorFieldSettingsContent>.PropertyAlias(x => x.Metal);
    
    public ZakatCalculatorFieldValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.EqualsInvariant(PlatformsConstants.Zakat.Settings.Calculator.Field.Alias);
    }

    public override void Validate(ContentProperties content) {
        var metal = content.Properties.Single(x => x.Alias.EqualsInvariant(MetalPropertyAlias)).Value;

        if (fieldType == ZakatCalculatorFieldTypes.Metal && !metal.HasValue()) {
            notification.CancelWithError("Metal must be specified");
        } else if (fieldType == ZakatCalculatorFieldTypes.Money && metal.HasValue()) {
            notification.CancelWithError("Metal cannot specified");
        }
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if () {
                var fieldType = GetZakatCalculatorFieldType(content);
                
            }
        }
        
        return Task.CompletedTask;
    }

    private ZakatCalculatorFieldType GetZakatCalculatorFieldType(ContentProperties content) {
        var fieldType = ContentHelper.GetDataListValue<ZakatCalculatorFieldType>(content,
                                                                                 AliasHelper<ZakatCalculatorFieldSettingsContent>.PropertyAlias(x => x.Type));
        
        return fieldType;
    }
}