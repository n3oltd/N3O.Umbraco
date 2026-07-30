using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler.Attributes;
using NodaTime;
using System;

namespace N3O.Umbraco.Scheduler;

public class BackgroundJob : IBackgroundJob {
    private readonly IFluentParametersBuilder _fluentParametersBuilder;
    private readonly IJsonProvider _jsonProvider;
    private readonly IJobSignatureProvider _jobSignatureProvider;
    private readonly IClock _clock;

    public BackgroundJob(IFluentParametersBuilder fluentParametersBuilder,
                         IJsonProvider jsonProvider,
                         IJobSignatureProvider jobSignatureProvider,
                         IClock clock) {
        _fluentParametersBuilder = fluentParametersBuilder;
        _jsonProvider = jsonProvider;
        _jobSignatureProvider = jobSignatureProvider;
        _clock = clock;
    }

    public string Enqueue<TRequest, TModel>(string jobName,
                                            TModel model,
                                            Action<IFluentParametersBuilder> addParameters = null,
                                            string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<TModel, None> where TModel : class {
        return Schedule<TRequest, TModel>(jobName, Duration.Zero, model, addParameters, queue);
    }

    public string Enqueue<TRequest>(string jobName,
                                    Action<IFluentParametersBuilder> addParameters = null,
                                    string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<None, None> {
        return Schedule<TRequest>(jobName, Duration.Zero, addParameters, queue);
    }

    public string Schedule<TRequest, TModel>(string jobName,
                                             Instant at,
                                             TModel model,
                                             Action<IFluentParametersBuilder> addParameters = null,
                                             string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<TModel, None>
        where TModel : class {
        var triggerKey = TriggerKey.Generate<TRequest, TModel>();

        addParameters?.Invoke(_fluentParametersBuilder);

        _fluentParametersBuilder.Add(SchedulerConstants.Parameters.Culture, LocalizationSettings.CultureCode);

        if (typeof(TRequest).HasAttribute<SignatureFencedAttribute>()) {
            _fluentParametersBuilder.Add(SchedulerConstants.Parameters.Queue, queue);
            _fluentParametersBuilder.Add(SchedulerConstants.Parameters.Signature,
                                         _jobSignatureProvider.GetSignature());
        }

        var parameterData = _fluentParametersBuilder.Build();
        var modelJson = _jsonProvider.SerializeObject(model);

        var enqueueAt = at.ToDateTimeOffset();
        var jobId = Hangfire.BackgroundJob.Schedule<JobTrigger>(queue,
                                                                j => j.TriggerAsync(jobName,
                                                                                    triggerKey,
                                                                                    modelJson,
                                                                                    parameterData),
                                                                enqueueAt);

        return jobId;
    }

    public string Schedule<TRequest, TModel>(string jobName,
                                             Duration fromNow,
                                             TModel model,
                                             Action<IFluentParametersBuilder> addParameters = null,
                                             string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<TModel, None>
        where TModel : class {
        if (fromNow.TotalSeconds < 0) {
            throw new ArgumentOutOfRangeException(nameof(fromNow), "Duration cannot be negative");
        }

        var at = _clock.GetCurrentInstant().Plus(fromNow);

        return Schedule<TRequest, TModel>(jobName, at, model, addParameters, queue);
    }

    public string Schedule<TRequest>(string jobName,
                                     Instant at,
                                     Action<IFluentParametersBuilder> addParameters = null,
                                     string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<None, None> {
        return Schedule<TRequest, None>(jobName, at, None.Empty, addParameters, queue);
    }

    public string Schedule<TRequest>(string jobName,
                                     Duration fromNow,
                                     Action<IFluentParametersBuilder> addParameters = null,
                                     string queue = SchedulerConstants.Queues.Default)
        where TRequest : Request<None, None> {
        return Schedule<TRequest, None>(jobName, fromNow, None.Empty, addParameters, queue);
    }

    public void Delete(string jobId) {
        Hangfire.BackgroundJob.Delete(jobId);
    }
}
