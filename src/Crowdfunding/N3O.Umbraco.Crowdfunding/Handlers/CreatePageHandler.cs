using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CreatePageHandler : IRequestHandler<CreatePageCommand, CreatePageReq, string> {
    private readonly IFundraisingPages _fundraisingPages;

    public CreatePageHandler(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    public Task<string> Handle(CreatePageCommand req, CancellationToken cancellationToken) {
        if (!_fundraisingPages.IsPageNameAvailable(req.Model.Name)) {
            throw new ValidationException(ValidationFailure.WithMessage(nameof(req.Model.Name),
                                                                        "Page already exists with the specified name"));
        }
        
        var result = _fundraisingPages.CreatePage(req);

        if (result.Success) {
            return Task.FromResult(result.Url);
        } else {
            throw new ValidationException(ValidationFailure.WithMessage(null, result.Error));
        }
    }
}