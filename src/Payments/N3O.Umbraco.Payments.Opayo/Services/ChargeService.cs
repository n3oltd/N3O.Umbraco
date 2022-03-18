using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Controllers;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo {
    public class ChargeService : IChargeService {
        private readonly IOpayoClient _opayoClient;
        private readonly IContentCache _contentCache;
        private readonly IActionLinkGenerator _actionLinkGenerator;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IBrowserInfoAccessor _browserInfoAccessor;

        public ChargeService(IOpayoClient opayoClient,
                             IContentCache contentCache,
                             IActionLinkGenerator actionLinkGenerator,
                             IRemoteIpAddressAccessor remoteIpAddressAccessor,
                             IBrowserInfoAccessor browserInfoAccessor) {
            _opayoClient = opayoClient;
            _contentCache = contentCache;
            _actionLinkGenerator = actionLinkGenerator;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _browserInfoAccessor = browserInfoAccessor;
        }

        public async Task ChargeAsync(OpayoPayment payment,
                                      ChargeCardReq req,
                                      PaymentsParameters parameters,
                                      bool saveCard) {
            try {
                var apiRequest = GetApiPaymentTransactionReq(req, parameters, saveCard);

                var transaction = await _opayoClient.TransactionAsync(apiRequest);
                
                payment.UpdateMerchantSessionKey(req.MerchantSessionKey);
                
                if (transaction.IsAuthorised()) {
                    payment.Paid(transaction.TransactionId,
                                 transaction.StatusCode,
                                 transaction.StatusDetail,
                                 transaction.BankAuthorisationCode,
                                 transaction.RetrievalReference.GetValueOrThrow());
                } else if (transaction.IsDeclined()) {
                    payment.Declined(transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
                } else if (transaction.IsRejected()) {
                    payment.Error(transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
                } else if (transaction.RequiresThreeDSecure()) {
                    payment.RequireThreeDSecure(transaction.TransactionId,
                                                req.ReturnUrl,
                                                transaction.AcsUrl,
                                                transaction.AcsTransId,
                                                transaction.CReq);
                } else {
                    throw UnrecognisedValueException.For(transaction.Status);
                }
            } catch (ApiException apiException) {
                var opayoErrors = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoErrors>);
                var opayoError = opayoErrors?.Errors.OrEmpty().FirstOrDefault() ??
                                 apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoError>);

                var errorMessage = opayoError?.ClientMessage ?? opayoError?.Description ?? opayoError?.StatusDetail;
                var errorCode = opayoError?.Code ?? opayoError?.StatusCode;
                var transactionId = opayoError?.TransactionId;

                payment.Error(transactionId, errorCode, errorMessage);
            }
        }

        private ApiPaymentTransactionReq GetApiPaymentTransactionReq(ChargeCardReq req,
                                                                     PaymentsParameters parameters,
                                                                     bool saveCard) {
            var settings = _contentCache.Single<OpayoSettingsContent>();
            var apiReq = new ApiPaymentTransactionReq();

            apiReq.Amount = ((Money) req.Value).GetAmountInLowestDenomination();
            apiReq.Currency = req.Value.Currency.Id;
            apiReq.Description =  parameters.GetTransactionDescription(settings);
            apiReq.VendorTxCode = parameters.GetTransactionId(settings, req.CardIdentifier).Left(40);

            var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();

            apiReq.BillingAddress = GetBillingAddress(billingInfo.Address);
            apiReq.ApplyThreeDSecure = "UseMSPSetting";
            apiReq.CustomerFirstName = GetText(billingInfo.Name.FirstName, 20, true);
            apiReq.CustomerLastName = GetText(billingInfo.Name.LastName, 20, true);

            apiReq.PaymentMethod = new ApiPaymentMethodReq();
            apiReq.PaymentMethod = GetApiPaymentMethodReq(req, saveCard);

            apiReq.EntryMethod = "Ecommerce";
            apiReq.TransactionType = "Payment";

            if (saveCard) {
                var apiCredentialTypeReq = new ApiCredentialTypeReq();
                apiCredentialTypeReq.MitType = "Unscheduled";
                apiCredentialTypeReq.InitiatedType = "CIT";
                apiCredentialTypeReq.CofUsage = "First";
                apiReq.CredentialType = apiCredentialTypeReq;
            }

            apiReq.StrongCustomerAuthentication = new ApiStrongCustomerAuthentication();
            apiReq.StrongCustomerAuthentication.BrowserIp = GetBrowserIpAddress();

            apiReq.StrongCustomerAuthentication.BrowserAcceptHeader = _browserInfoAccessor.GetAccept();
            apiReq.StrongCustomerAuthentication.BrowserJavascriptEnabled = req.BrowserParameters
                                                                              .JavaScriptEnabled
                                                                              .GetValueOrThrow();

            apiReq.StrongCustomerAuthentication.BrowserJavaEnabled = req.BrowserParameters
                                                                        .JavaEnabled
                                                                        .GetValueOrThrow();

            apiReq.StrongCustomerAuthentication.BrowserLanguage = _browserInfoAccessor.GetLanguage();
            apiReq.StrongCustomerAuthentication.BrowserScreenHeight = req.BrowserParameters.ScreenHeight.ToString();
            apiReq.StrongCustomerAuthentication.BrowserScreenWidth = req.BrowserParameters.ScreenWidth.ToString();
            apiReq.StrongCustomerAuthentication.BrowserUserAgent = _browserInfoAccessor.GetUserAgent();
            apiReq.StrongCustomerAuthentication.ChallengeWindowSize = req.ChallengeWindowSize.Name;
            apiReq.StrongCustomerAuthentication.BrowserColorDepth = req.BrowserParameters.GetColourDepth().ToString();
            apiReq.StrongCustomerAuthentication.BrowserTimezone = req.BrowserParameters.UtcOffsetMinutes.ToString();
            apiReq.StrongCustomerAuthentication.NotificationUrl = GetNotificationUrl(parameters.FlowId);
            apiReq.StrongCustomerAuthentication.TransactionType = "GoodsAndServicePurchase";

            return apiReq;
        }

        private string GetBrowserIpAddress() {
            var ipAddress = _remoteIpAddressAccessor.GetRemoteIpAddress();

            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) {
                return ipAddress.ToString();
            } else {
                // Opayo do not yet support IPv6 Addresses so use our IP address in these cases until Opayo
                // confirm support has been added
                return "45.77.226.187";
            }
        }

        private string GetNotificationUrl(EntityId flowId) {
            return _actionLinkGenerator.GetUrl<OpayoController>(x => x.CompleteThreeDSecureChallenge(null),
                                                                new { flowId = flowId.ToString() });
        }

        private string GetText(string value, int maxLength, bool required, string defaultValue = ".") {
            if (!value.HasValue() && required) {
                value = defaultValue;
            }

            if (value == null) {
                return null;
            }

            return value.RemoveNonAscii().Trim().Right(maxLength);
        }

        private ApiBillingAddressReq GetBillingAddress(IAddress address) {
            var billingAddress = new ApiBillingAddressReq();
            billingAddress.Address1 = GetText(address.Line1, 50, true);
            billingAddress.Address2 = GetText(address.Line2, 50, false);
            billingAddress.Address3 = GetText(address.Line3, 50, false);
            billingAddress.City = GetText(address.Locality, 40, true);
            
            if (address.Country.Iso3Code.EqualsInvariant(OpayoConstants.Iso3CountryCodes.UnitedStates)) {
                billingAddress.State = GetUsStateCode(address.AdministrativeArea);
            }

            billingAddress.PostalCode = GetText(address.PostalCode, 10, true, "0000");
            billingAddress.Country = address.Country.Iso2Code;

            return billingAddress;
        }

        private ApiPaymentMethodReq GetApiPaymentMethodReq(ChargeCardReq req, bool saveCard) {
            var apiCard = new ApiCard();
            apiCard.CardIdentifier = req.CardIdentifier;
            apiCard.MerchantSessionKey = req.MerchantSessionKey;
            apiCard.Reusable = false;
            apiCard.Save = saveCard;

            var apiPaymentMethodReq = new ApiPaymentMethodReq();
            apiPaymentMethodReq.Card = apiCard;

            return apiPaymentMethodReq;
        }

        private static string GetUsStateCode(string administrativeArea) {
            if (UsStates.ContainsKey(administrativeArea)) {
                return administrativeArea.ToUpper();
            }

            foreach (var (code, name) in UsStates) {
                if (name.EqualsInvariant(administrativeArea)) {
                    return code;
                }
            }

            // Safe default for SagePay
            return "NY";
        }
        
        private static readonly Dictionary<string, string> UsStates = new(StringComparer.InvariantCultureIgnoreCase) {
                { "AA", "Armed Forces America" },
                { "AE", "Armed Forces" },
                { "AK", "Alaska" },
                { "AL", "Alabama" },
                { "AP", "Armed Forces Pacific" },
                { "AR", "Arkansas" },
                { "AZ", "Arizona" },
                { "CA", "California" },
                { "CO", "Colorado" },
                { "CT", "Connecticut" },
                { "DC", "Washington DC" },
                { "DE", "Delaware" },
                { "FL", "Florida" },
                { "GA", "Georgia" },
                { "GU", "Guam" },
                { "HI", "Hawaii" },
                { "IA", "Iowa" },
                { "ID", "Idaho" },
                { "IL", "Illinois" },
                { "IN", "Indiana" },
                { "KS", "Kansas" },
                { "KY", "Kentucky" },
                { "LA", "Louisiana" },
                { "MA", "Massachusetts" },
                { "MD", "Maryland" },
                { "ME", "Maine" },
                { "MI", "Michigan" },
                { "MN", "Minnesota" },
                { "MO", "Missouri" },
                { "MS", "Mississippi" },
                { "MT", "Montana" },
                { "NC", "North Carolina" },
                { "ND", "North Dakota" },
                { "NE", "Nebraska" },
                { "NH", "New Hampshire" },
                { "NJ", "New Jersey" },
                { "NM", "New Mexico" },
                { "NV", "Nevada" },
                { "NY", "New York" },
                { "OH", "Ohio" },
                { "OK", "Oklahoma" },
                { "OR", "Oregon" },
                { "PA", "Pennsylvania" },
                { "PR", "Puerto Rico" },
                { "RI", "Rhode Island" },
                { "SC", "South Carolina" },
                { "SD", "South Dakota" },
                { "TN", "Tennessee" },
                { "TX", "Texas" },
                { "UT", "Utah" },
                { "VA", "Virginia" },
                { "VI", "Virgin Islands" },
                { "VT", "Vermont" },
                { "WA", "Washington" },
                { "WI", "Wisconsin" },
                { "WV", "West Virginia" },
                { "WY", "Wyoming" }
            };
    }
}