using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Entities;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.References;
using N3O.Umbraco.Scheduler;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class CreateExportHandler : IRequestHandler<CreateExportCommand, ExportReq, ExportProgressRes> {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IRepository<Export> _repository;
    private readonly IUmbracoMapper _mapper;
    private readonly ICounters _counters;
    private readonly IContentTypeService _contentTypeService;

    public CreateExportHandler(IBackgroundJob backgroundJob,
                               IRepository<Export> repository,
                               IUmbracoMapper mapper,
                               ICounters counters,
                               IContentTypeService contentTypeService) {
        _backgroundJob = backgroundJob;
        _repository = repository;
        _mapper = mapper;
        _counters = counters;
        _contentTypeService = contentTypeService;
    }

    public async Task<ExportProgressRes> Handle(CreateExportCommand req, CancellationToken cancellationToken) {
        var contentType = _contentTypeService.Get(req.ContentType.Value);

        var exportId = Guid.NewGuid();
        var export = await Export.CreateAsync(_counters,
                                              exportId,
                                              contentType,
                                              req.ContainerId.Value,
                                              req.Model.Format);

        await _repository.InsertAsync(export);

        _backgroundJob.Enqueue<ProcessExportCommand, ExportReq>($"ProcessExport({exportId})",
                                                                req.Model,
                                                                p => p.Add<ExportId>(exportId.ToString()), 
                                                                SchedulerConstants.Queues.LongJobs);

        var res = _mapper.Map<Export, ExportProgressRes>(export);

        return res;
    }
}
