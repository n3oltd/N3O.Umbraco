using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;

    public JobTrigger(IServiceScopeFactory serviceScopeFactory,
                      IVariationContextAccessor variationContextAccessor,
                      ILocalizationSettingsAccessor localizationSettingsAccessor) {
        _serviceScopeFactory = serviceScopeFactory;
        _variationContextAccessor = variationContextAccessor;
        _localizationSettingsAccessor = localizationSettingsAccessor;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        var requestType = TriggerKey.ParseRequestType(triggerKey);

        SetRequestCulture(parameterData);

        using (var scope = _serviceScopeFactory.CreateScope()) {
            var umbracoContextFactory = scope.ServiceProvider.GetRequiredService<IUmbracoContextFactory>();
                
            using (umbracoContextFactory.EnsureUmbracoContext()) {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var fluentParameters = scope.ServiceProvider.GetRequiredService<IFluentParameters>();
                var jsonProvider = scope.ServiceProvider.GetRequiredService<IJsonProvider>();
                    
                var modelType = TriggerKey.ParseModelType(triggerKey);
                    
                var model = jsonProvider.DeserializeObject(modelJson, modelType);

                foreach (var (name, value) in parameterData.OrEmpty()) {
                    fluentParameters.Add(name, value);
                }
                    
                await mediator.SendAsync(requestType, typeof(None), model);
            }
        }
    }

    private void SetRequestCulture(IReadOnlyDictionary<string, string> parameterData) {
        var culture = parameterData.ContainsKey(SchedulerConstants.Parameters.Culture) 
                          ? parameterData[SchedulerConstants.Parameters.Culture] 
                          : _localizationSettingsAccessor.GetSettings().DefaultCultureCode;
        
        _variationContextAccessor.VariationContext = new VariationContext(culture);
    }

    private string GetActivityName(Type requestType) {
        var typeName = requestType.Name.Camelize();

        if (typeName.EndsWith("Command")) {
            typeName = typeName.Substring(0, typeName.Length - "Command".Length);
        }
        
        return $"scheduler/{typeName}";
    }
}
