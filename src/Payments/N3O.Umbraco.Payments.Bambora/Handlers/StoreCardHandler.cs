using N3O.Umbraco.Context;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Bambora.Clients;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Extensions;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Handlers;

public class StoreCardHandler : PaymentsHandler<StoreCardCommand, StoreCardReq, BamboraCredential> {
    private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
    private readonly IActionLinkGenerator _actionLinkGenerator;
    private readonly IBrowserInfoAccessor _browserInfoAccessor;
    private readonly IBamboraProfilesClient _profilesClient;

    public StoreCardHandler(IPaymentsScope paymentsScope,
                            IRemoteIpAddressAccessor remoteIpAddressAccessor,
                            IActionLinkGenerator actionLinkGenerator,
                            IBrowserInfoAccessor browserInfoAccessor,
                            IBamboraProfilesClient profilesClient)
        : base(paymentsScope) {
        _remoteIpAddressAccessor = remoteIpAddressAccessor;
        _actionLinkGenerator = actionLinkGenerator;
        _browserInfoAccessor = browserInfoAccessor;
        _profilesClient = profilesClient;
    }

    protected override async Task HandleAsync(StoreCardCommand req,
                                              BamboraCredential credential,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        try {
            var apiRequest = GetRequest(req.Model, parameters);

            var apiProfile = await _profilesClient.CreateProfileAsync(apiRequest);

            credential.UpdateToken(req.Model.Token);

            if (apiProfile.IsSuccessful()) {
                credential.SetUp(apiProfile.CustomerCode, apiProfile.Message, apiProfile.Code);
            } else {
                throw UnrecognisedValueException.For(apiProfile.Code);
            }
        } catch (ApiException apiException) {
            var apiPaymentError = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<ApiPaymentError>);

            credential.Error(apiPaymentError.Code, apiPaymentError.Message);
        }
    }

    private ApiProfileReq GetRequest(StoreCardReq req, PaymentsParameters parameters) {
        var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();

        var apiReq = new ApiProfileReq();
        apiReq.BillingAddress = billingInfo.GetApiBillingAddress();
        apiReq.CustomerIp =  _remoteIpAddressAccessor.GetRemoteIpAddress().ToString();
        apiReq.Token = new Token();
        apiReq.Token.Code = req.Token;
        apiReq.Token.Complete = true;
        apiReq.Token.Name = billingInfo.Name.FirstName;
        apiReq.Token.ThreeDSecure = new ThreeDSecure();
        apiReq.Token.ThreeDSecure.Enabled = true;
        apiReq.Token.ThreeDSecure.Version = 2;
        apiReq.Token.ThreeDSecure.AuthRequired = false;
        apiReq.Token.ThreeDSecure.Browser = _browserInfoAccessor.GetBrowserReq(req.BrowserParameters);
        apiReq.ReturnUrl = _actionLinkGenerator.GetPaymentThreeDSecureUrl(parameters.FlowId);
        
        return apiReq;
    }
}
