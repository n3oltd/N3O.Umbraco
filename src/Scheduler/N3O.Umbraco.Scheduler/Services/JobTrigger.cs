using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Telemetry;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

// Lack of interface is by design, want Hangfire to only have concrete type
public class JobTrigger {
    private const string Category = "Scheduler";
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
                
                var activitySource = scope.ServiceProvider.GetService<ActivitySource>();
                var durationWeightFinder = scope.ServiceProvider.GetService<IDurationWeightFinder>();

                using var activity = activitySource.StartTimedActivity(durationWeightFinder, requestType.Name, Category);

                await mediator.SendAsync(requestType, typeof(None), model);
                
                activity?.Stop();
            }
        }
    }
}
