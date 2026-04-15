using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace N3O.Umbraco.KeyVault;

// Certain third party configuration settings use underscores but these are not supported in key vault secret names
public class CustomKeyVaultSecretManager : KeyVaultSecretManager {
    public override string GetKey(KeyVaultSecret secret) {
        return secret.Name.Replace("-", "_");
    }
}
