using System.Collections.Generic;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients.Loqate;

public class ValidateResponse {
    public IEnumerable<ValidateResult> Items { get; set; }
}