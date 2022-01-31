using GoCardless;
using GoCardless.Services;
using N3O.Umbraco.Content;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Extensions;
using N3O.Umbraco.Payments.GoCardless.Commands;
using N3O.Umbraco.Payments.GoCardless.Content;
using N3O.Umbraco.Payments.GoCardless.Controllers;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Handlers {
    public class BeginRedirectFlowHandler : PaymentsHandler<BeginRedirectFlowCommand, None, GoCardlessCredential> {
        private readonly IContentCache _contentCache;
        private readonly GoCardlessClient _goCardlessClient;
        private readonly IActionLinkGenerator _actionLinkGenerator;

        public BeginRedirectFlowHandler(IContentCache contentCache,
                                        GoCardlessClient goCardlessClient,
                                        IPaymentsScope paymentsScope,
                                        IActionLinkGenerator actionLinkGenerator)
            : base(paymentsScope) {
            _contentCache = contentCache;
            _goCardlessClient = goCardlessClient;
            _actionLinkGenerator = actionLinkGenerator;
        }

        protected override async Task HandleAsync(BeginRedirectFlowCommand req,
                                                  GoCardlessCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            var request = GetRedirectFlowCreateRequest(parameters, req.FlowId.Value);
            var redirectFlowResponse = await _goCardlessClient.RedirectFlows.CreateAsync(request);
            var redirectFlow = redirectFlowResponse.RedirectFlow;
            
            credential.BeginRedirectFlow(redirectFlow.Id, redirectFlow.SessionToken);
        }

        private RedirectFlowCreateRequest GetRedirectFlowCreateRequest(PaymentsParameters parameters, Guid flowId) {
            var settings = _contentCache.Single<GoCardlessSettingsContent>();

            var createRequest = new RedirectFlowCreateRequest();
            createRequest.Description = settings.GetTransactionDescription(parameters.Reference);
            createRequest.SessionToken = Guid.NewGuid().ToString();
            createRequest.SuccessRedirectUrl = _actionLinkGenerator.GetUrl<GoCardlessController>(x => x.Complete(),
                                                                                                 new { flowId });
            createRequest.PrefilledCustomer = GetPrefilledCustomer(parameters.BillingInfoAccessor);

            return createRequest;
        }

        private RedirectFlowCreateRequest.RedirectFlowPrefilledCustomer GetPrefilledCustomer(IBillingInfoAccessor billingInfoAccessor) {
            var customer = new RedirectFlowCreateRequest.RedirectFlowPrefilledCustomer();

            var billingInfo = billingInfoAccessor.GetBillingInfo();

            customer.GivenName = billingInfo.Name.FirstName;
            customer.FamilyName = billingInfo.Name.LastName;
            customer.AddressLine1 = billingInfo.Address.Line1;
            customer.AddressLine2 = billingInfo.Address.Line2;
            customer.AddressLine3 = billingInfo.Address.Line3;
            customer.City = billingInfo.Address.Locality;
            customer.Region = billingInfo.Address.AdministrativeArea;
            customer.PostalCode = billingInfo.Address.PostalCode;
            customer.CountryCode = billingInfo.Address.Country.Iso2Code;
            customer.Email = billingInfo.Email.Address;
            customer.PhoneNumber = billingInfo.Telephone?.Number;

            return customer;
        }
    }
}