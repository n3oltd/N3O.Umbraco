using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Telemetry;
using N3O.Umbraco.Telemetry.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JobTrigger(IServiceScopeFactory serviceScopeFactory) {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        var requestType = TriggerKey.ParseRequestType(triggerKey);

        using (var scope = _serviceScopeFactory.CreateScope()) {
            using (var activity = ActivitySources.Get<JobTrigger>().StartTimedActivity(GetActivityName(requestType), "scheduler")) {
                activity.AddBaggage("triggerKey", triggerKey);
                
                activity.AddTag("jobType", requestType.FullName);
                activity.AddTag("jobName", jobName);
                activity.AddTag("modelJson", modelJson);

                var umbracoContextFactory = scope.ServiceProvider.GetRequiredService<IUmbracoContextFactory>();
                
                using (umbracoContextFactory.EnsureUmbracoContext()) {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var fluentParameters = scope.ServiceProvider.GetRequiredService<IFluentParameters>();
                    var jsonProvider = scope.ServiceProvider.GetRequiredService<IJsonProvider>();
                    
                    var modelType = TriggerKey.ParseModelType(triggerKey);
                    
                    var model = jsonProvider.DeserializeObject(modelJson, modelType);

                    foreach (var (name, value) in parameterData.OrEmpty()) {
                        fluentParameters.Add(name, value);

                        activity.AddTag($"parameter{name.Pascalize()}", value);
                    }
                    
                    await mediator.SendAsync(requestType, typeof(None), model);
                }
            }
        }
    }
    
    private string GetActivityName(Type requestType) {
        var typeName = requestType.Name.Camelize();

        if (typeName.EndsWith("Command")) {
            typeName = typeName.Substring(0, typeName.Length - "Command".Length);
        }
        
        return $"scheduler/{typeName}";
    }
}
