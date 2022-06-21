namespace N3O.Umbraco.Payments.Models;

public partial class Credential {
    protected void SetUp() {
        IsSetUp = true;
        SetupAt = Clock.GetCurrentInstant();

        Complete();
    }
}
