using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class ProcessPaymentCommand : Request<OpayoPaymentReq, OpayoPaymentRes> { }
}