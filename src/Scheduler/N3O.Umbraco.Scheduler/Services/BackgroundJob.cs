using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using NodaTime;
using System;

namespace N3O.Umbraco.Scheduler;

public class BackgroundJob : IBackgroundJob {
    private readonly IFluentParametersBuilder _fluentParametersBuilder;
    private readonly IJsonProvider _jsonProvider;
    private readonly IClock _clock;

    public BackgroundJob(IFluentParametersBuilder fluentParametersBuilder,
                         IJsonProvider jsonProvider,
                         IClock clock) {
        _fluentParametersBuilder = fluentParametersBuilder;
        _jsonProvider = jsonProvider;
        _clock = clock;
    }

    public string Enqueue<TRequest, TModel>(string jobName,
                                            TModel model,
                                            Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None> where TModel : class {
        return Schedule<TRequest, TModel>(jobName, Duration.Zero, model, addParameters);
    }

    public string Enqueue<TRequest>(string jobName, Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None> {
        return Schedule<TRequest>(jobName, Duration.Zero, addParameters);
    }

    public string Schedule<TRequest, TModel>(string jobName,
                                             Instant at,
                                             TModel model,
                                             Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None>
        where TModel : class {
        var triggerKey = TriggerKey.Generate<TRequest, TModel>();

        addParameters?.Invoke(_fluentParametersBuilder);

        _fluentParametersBuilder.Add(SchedulerConstants.Parameters.Culture, LocalizationSettings.CultureCode);

        var parameterData = _fluentParametersBuilder.Build();
        var modelJson = _jsonProvider.SerializeObject(model);

        var enqueueAt = at.ToDateTimeOffset();
        var jobId = Hangfire.BackgroundJob.Schedule<JobTrigger>(j => j.TriggerAsync(jobName,
                                                                                    triggerKey,
                                                                                    modelJson,
                                                                                    parameterData),
                                                                enqueueAt);

        return jobId;
    }

    public string Schedule<TRequest, TModel>(string jobName,
                                             Duration fromNow,
                                             TModel model,
                                             Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None>
        where TModel : class {
        if (fromNow.TotalSeconds < 0) {
            throw new ArgumentOutOfRangeException(nameof(fromNow), "Duration cannot be negative");
        }

        var at = _clock.GetCurrentInstant().Plus(fromNow);

        return Schedule<TRequest, TModel>(jobName, at, model, addParameters);
    }

    public string Schedule<TRequest>(string jobName,
                                     Instant at,
                                     Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None> {
        return Schedule<TRequest, None>(jobName, at, None.Empty, addParameters);
    }

    public string Schedule<TRequest>(string jobName,
                                     Duration fromNow,
                                     Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None> {
        return Schedule<TRequest, None>(jobName, fromNow, None.Empty, addParameters);
    }

    public void Delete(string jobId) {
        Hangfire.BackgroundJob.Delete(jobId);
    }
}
