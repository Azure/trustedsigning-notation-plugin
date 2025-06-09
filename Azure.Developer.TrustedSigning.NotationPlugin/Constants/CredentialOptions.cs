// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Constants;

internal static class CredentialOptions
{
    public const string Environment = "EnvironmentCredential";
    public const string ManagedIdentity = "ManagedIdentityCredential";
    public const string SharedTokenCache = "SharedTokenCacheCredential";
    public const string VisualStudio = "VisualStudioCredential";
    public const string AzureCli = "AzureCliCredential";
    public const string AzurePowershell = "AzurePowerShellCredential";
    public const string InteractiveBrowser = "InteractiveBrowserCredential";
    public const string WorkloadIdentity = "WorkloadIdentityCredential";
    public const string AzureDeveloperCli = "AzureDeveloperCliCredential";

    public static readonly ImmutableHashSet<string> SupportedTypes = [
        Environment,
        ManagedIdentity,
        SharedTokenCache,
        VisualStudio,
        AzureCli,
        AzurePowershell,
        InteractiveBrowser,
        WorkloadIdentity,
        AzureDeveloperCli
    ];
}
