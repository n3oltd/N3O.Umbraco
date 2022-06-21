namespace N3O.Umbraco.Payments.Bambora.Models;

public partial class BamboraCredential {
    public void UpdateToken(string token) {
        BamboraToken = token;
    }
}
