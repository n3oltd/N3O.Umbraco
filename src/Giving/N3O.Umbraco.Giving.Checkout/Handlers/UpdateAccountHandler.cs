using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Analytics;
using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.TaxRelief;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Handlers;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountReq, CheckoutRes> {
    private readonly IRepository<Entities.Checkout> _repository;
    private readonly IUmbracoMapper _mapper;
    private readonly IContentCache _contentCache;
    private readonly IAttributionAccessor _attributionAccessor;
    private readonly ITaxReliefSchemeAccessor _taxReliefSchemeAccessor;

    public UpdateAccountHandler(IRepository<Entities.Checkout> repository,
                                IUmbracoMapper mapper,
                                IContentCache contentCache,
                                IAttributionAccessor attributionAccessor,
                                ITaxReliefSchemeAccessor taxReliefSchemeAccessor) {
        _repository = repository;
        _mapper = mapper;
        _contentCache = contentCache;
        _attributionAccessor = attributionAccessor;
        _taxReliefSchemeAccessor = taxReliefSchemeAccessor;
    }

    public async Task<CheckoutRes> Handle(UpdateAccountCommand req, CancellationToken cancellationToken) {
        var checkout = await req.CheckoutRevisionId.RunAsync(_repository.GetAsync, true, cancellationToken);

        checkout.UpdateAccount(_contentCache, _taxReliefSchemeAccessor, account => {
            if (req.Model.Individual.HasValue()) {
                account = account.WithUpdatedIndividual(req.Model.Individual);
            }

            if (req.Model.Organization.HasValue()) {
                account = account.WithUpdatedOrganization(req.Model.Organization);
            }

            if (req.Model.Type.HasValue()) {
                account = account.WithUpdatedType(req.Model.Type);
            }

            if (req.Model.Address.HasValue()) {
                account = account.WithUpdatedAddress(req.Model.Address);
            }

            if (req.Model.Email.HasValue()) {
                account = account.WithUpdatedEmail(req.Model.Email);
            }

            if (req.Model.Telephone.HasValue()) {
                account = account.WithUpdatedTelephone(req.Model.Telephone);
            }

            if (req.Model.Consent.HasValue()) {
                account = account.WithUpdatedConsent(req.Model.Consent);
            }

            if (req.Model.TaxStatus.HasValue()) {
                account = account.WithUpdatedTaxStatus(req.Model.TaxStatus);
            }

            return account;
        });

        checkout.UpdateAttribution(_attributionAccessor);
        
        await _repository.UpdateAsync(checkout);

        var res = _mapper.Map<Entities.Checkout, CheckoutRes>(checkout);

        return res;
    }
}