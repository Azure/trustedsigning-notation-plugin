// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.CryptoProvider;
using Azure.Developer.TrustedSigning.CryptoProvider.Interfaces;
using Azure.Identity;
using Azure.Developer.TrustedSigning.NotationPlugin.Constants;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation:  https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#generate-signature
/// </summary>
internal class SignatureRequest : BaseRequest
{
    public required string ContractVersion { get; set; }
    public required string KeyId { get; set; }
    public required string KeySpec { get; set; }
    public required string HashAlgorithm { get; set; }
    public required string Payload { get; set; }

    internal virtual ISignContext GetAzSignContext()
    {
        if (PluginConfig is null)
        {
            throw new NotationException("pluginConfig from request is null", ErrorCodes.ValidationError);
        }

        DefaultAzureCredential credential = GetAzureCredential(PluginConfig.ExcludeCredentials);

        // Set up TrustedSigning session
        return new AzSignContext(
            credential,
            PluginConfig.AccountName,
            PluginConfig.CertProfile,
            new Uri(PluginConfig.BaseUrl),
            clientVersion: PluginConstants.ClientVersionString
        );
    }

    private static DefaultAzureCredential GetAzureCredential(string? excludeCredentials)
    {
        if (string.IsNullOrWhiteSpace(excludeCredentials))
        {
            return new DefaultAzureCredential();
        }

        var providedCredentials = excludeCredentials.Split(',').Select(x => x.Trim()).ToList();
        var stringComparer = StringComparer.OrdinalIgnoreCase;

        foreach (string item in providedCredentials)
        {
            if (!CredentialOptions.SupportedTypes.Contains(item, stringComparer))
            {
                throw new NotationException($"The user-supplied ExcludeCredentials type {item} is not valid. Valid values are: {string.Join(", ", CredentialOptions.SupportedTypes)}", ErrorCodes.ValidationError);
            }
        }

        DefaultAzureCredentialOptions authOptions = new()
        {
            ExcludeEnvironmentCredential = providedCredentials.Contains(CredentialOptions.Environment, stringComparer),
            ExcludeManagedIdentityCredential = providedCredentials.Contains(CredentialOptions.ManagedIdentity, stringComparer),
#pragma warning disable CS0618 // Shared token credentials has been deprecated on Azure.Identity 1.15.0
            ExcludeSharedTokenCacheCredential = providedCredentials.Contains(CredentialOptions.SharedTokenCache, stringComparer),
#pragma warning restore CS0618 // Shared token credentials has been deprecated on Azure.Identity 1.15.0
            ExcludeVisualStudioCredential = providedCredentials.Contains(CredentialOptions.VisualStudio, stringComparer),
            ExcludeAzureCliCredential = providedCredentials.Contains(CredentialOptions.AzureCli, stringComparer),
            ExcludeAzurePowerShellCredential = providedCredentials.Contains(CredentialOptions.AzurePowershell, stringComparer),
            ExcludeInteractiveBrowserCredential = providedCredentials.Contains(CredentialOptions.InteractiveBrowser, stringComparer),
            ExcludeWorkloadIdentityCredential = providedCredentials.Contains(CredentialOptions.WorkloadIdentity, stringComparer),
            ExcludeAzureDeveloperCliCredential = providedCredentials.Contains(CredentialOptions.AzureDeveloperCli, stringComparer)
        };

        return new DefaultAzureCredential(authOptions);
    }
}
