using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler;

namespace N3O.Umbraco.Email {
    public class EmailBuilder : IEmailBuilder {
        private readonly IBackgroundJob _backgroundJob;
        private readonly IJsonProvider _jsonProvider;

        public EmailBuilder(IBackgroundJob backgroundJob, IJsonProvider jsonProvider) {
            _backgroundJob = backgroundJob;
            _jsonProvider = jsonProvider;
        }
        
        public IFluentEmailBuilder<T> Create<T>() {
            return new FluentEmailBuilder<T>(_backgroundJob, _jsonProvider);
        }
    }
}
