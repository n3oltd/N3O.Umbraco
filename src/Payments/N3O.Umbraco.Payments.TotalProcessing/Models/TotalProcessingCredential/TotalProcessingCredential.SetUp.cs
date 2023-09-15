namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingCredential {
    public void SetUp(string regisrationId) {
        ClearErrors();

        TotalProcessingRegistrationId = regisrationId;

        SetUp();
    }
}
