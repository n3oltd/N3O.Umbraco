using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Telemetry.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private static readonly ActivitySource ActivitySource = new(typeof(JobTrigger).Assembly.FullName);
    
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JobTrigger(IServiceScopeFactory serviceScopeFactory) {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        using (var scope = _serviceScopeFactory.CreateScope()) {
            using (var activity = ActivitySource.StartTimedActivity("RunScheduledJob", "Scheduler")) {
                var umbracoContextFactory = scope.ServiceProvider.GetRequiredService<IUmbracoContextFactory>();
                
                using (umbracoContextFactory.EnsureUmbracoContext()) {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var fluentParameters = scope.ServiceProvider.GetRequiredService<IFluentParameters>();
                    var jsonProvider = scope.ServiceProvider.GetRequiredService<IJsonProvider>();

                    var requestType = TriggerKey.ParseRequestType(triggerKey);
                    var modelType = TriggerKey.ParseModelType(triggerKey);
                    var model = jsonProvider.DeserializeObject(modelJson, modelType);

                    foreach (var (name, value) in parameterData.OrEmpty()) {
                        fluentParameters.Add(name, value);
                    }
                
                    activity.AddTag("RequestType", requestType.FullName);
                    activity.AddTag("JobName", jobName);
                    activity.AddTag("TriggerKey", triggerKey);
                    
                    await mediator.SendAsync(requestType, typeof(None), model);
                }
            }
        }
    }
}
