using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Criteria;
using N3O.Umbraco.Payments.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Queries;

public class FindPaymentMethodsQuery : Request<PaymentMethodCriteria, IEnumerable<PaymentMethodRes>> { }
