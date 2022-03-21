using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Handlers {
    public class ChargeCardHandler : PaymentsHandler<ChargeCardCommand, ChargeCardReq, BamboraPayment> {
        private readonly IBamboraClient _bamboraClient;

        public ChargeCardHandler(IPaymentsScope paymentsScope, IBamboraClient bamboraClient) : base(paymentsScope) {
            _bamboraClient = bamboraClient;
        }

        protected override async Task HandleAsync(ChargeCardCommand req,
                                                  BamboraPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            var apiPaymentReq = GetRequest(req.Model, parameters);

            var res = await _bamboraClient.CreatePaymentAsync(apiPaymentReq);
        }

        private ApiPaymentReq GetRequest(ChargeCardReq req, PaymentsParameters parameters) {
            var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
            var request = new ApiPaymentReq() {
                Amount = req.Value.Amount.GetValueOrThrow(),
                Billing = GetBillingAddress(billingInfo),
                PaymentMethod = "token",
                Token = new Token() {
                    Code = req.Token,
                    Complete = true,
                    Name = billingInfo.Name.FirstName
                }
            };

            return request;
        }

        private ApiBillingAddressReq GetBillingAddress(BillingInfo billingInfo) {
            var address = billingInfo.Address;

            var billingAddress = new ApiBillingAddressReq();
            billingAddress.AddressLine1 = GetText(address.Line1, 50, true);
            billingAddress.AddressLine2 = GetText(address.Line2, 50, false);
            billingAddress.City = GetText(address.Locality, 40, true);
            billingAddress.EmailAddress = billingInfo.Email.Address;
            billingAddress.PostalCode = GetText(address.PostalCode, 10, true, "0000");
            billingAddress.Country = address.Country.Iso2Code;

            return billingAddress;
        }

        private string GetText(string value, int maxLength, bool required, string defaultValue = ".") {
            if (!value.HasValue() && required) {
                value = defaultValue;
            }

            if (value == null) {
                return null;
            }

            return value.RemoveNonAscii()
                        .Trim()
                        .Right(maxLength);
        }
    }
}