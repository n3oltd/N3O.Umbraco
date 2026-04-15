using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace N3O.Umbraco.KeyVault;

public class CustomKeyVaultSecretManager : KeyVaultSecretManager {
    public override string GetKey(KeyVaultSecret secret) {
        return secret.Name.Replace("-", "_");
    }
}
