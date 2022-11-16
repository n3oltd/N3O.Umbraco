using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Entities;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Handlers;

public class GetExportProgressHandler : IRequestHandler<GetExportProgressQuery, None, ExportProgressRes> {
    private readonly IRepository<Export> _repository;
    private readonly IUmbracoMapper _mapper;

    public GetExportProgressHandler(IRepository<Export> repository, IUmbracoMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExportProgressRes> Handle(GetExportProgressQuery req, CancellationToken cancellationToken) {
        var export = await req.ExportId.RunAsync(_repository.GetAsync, true, cancellationToken);

        var res = _mapper.Map<Export, ExportProgressRes>(export);

        return res;
    }
}