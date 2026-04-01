using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;

namespace N3O.Umbraco.KeyVault.Extensions;

public class KeyVaultConfigurationBuilderExtension : IConfigurationBuilderExtension {
    public void Run(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context) {
        var config = configurationBuilder.Build();
        var keyVaultUrl = config["AzureKeyVaultUrl"];

        if (keyVaultUrl.HasValue() && keyVaultUrl.IsValidUrl()) {
            configurationBuilder.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }
    }
}