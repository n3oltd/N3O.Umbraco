using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using NodaTime;
using System;

namespace N3O.Umbraco.Scheduler;

public interface IBackgroundJob {
    string Enqueue<TRequest, TModel>(string jobName,
                                     TModel model,
                                     Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None>
        where TModel : class;

    string Enqueue<TRequest>(string jobName, Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None>;

    string Schedule<TRequest>(string jobName, Instant at, Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None>;

    string Schedule<TRequest>(string jobName,
                              Duration fromNow,
                              Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<None, None>;

    string Schedule<TRequest, TModel>(string jobName,
                                      Instant at,
                                      TModel model,
                                      Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None>
        where TModel : class;

    string Schedule<TRequest, TModel>(string jobName,
                                      Duration fromNow,
                                      TModel model,
                                      Action<IFluentParametersBuilder> addParameters = null)
        where TRequest : Request<TModel, None>
        where TModel : class;

    void Delete(string jobId);
}
