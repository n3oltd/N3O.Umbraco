using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler {
    // Lack of interface is by design, want Hangfire to only have concrete type
    public class JobTrigger {
        private readonly IMediator _mediator;
        private readonly IFluentParameters _fluentParameters;
        private readonly IJsonProvider _jsonProvider;

        public JobTrigger(IMediator mediator, IFluentParameters fluentParameters, IJsonProvider jsonProvider) {
            _mediator = mediator;
            _fluentParameters = fluentParameters;
            _jsonProvider = jsonProvider;
        }
    
        [DisplayName("{0}")]
        public async Task TriggerAsync(string jobName,
                                       string triggerKey,
                                       string modelJson,
                                       IReadOnlyDictionary<string, string> parameterData) {
            var requestType = TriggerKey.ParseRequestType(triggerKey);
            var modelType = TriggerKey.ParseModelType(triggerKey);
            var model = _jsonProvider.DeserializeObject(modelJson, modelType);
        
            foreach (var (name, value) in parameterData.OrEmpty()) {
                _fluentParameters.Add(name, value);
            }

            await _mediator.SendAsync(requestType, typeof(None), model);
        }
    }
}
