// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

internal static class Validator
{
    public static bool ValidateMetadataRequest([NotNullWhen(true)] MetadataRequest? request, out string errorMessage)
    {
        errorMessage = "";

        if (request == null)
        {
            errorMessage = "Request from notation was null";
            return false;
        }

        return true;
    }

    public static bool ValidateKeyRequest([NotNullWhen(true)] KeyRequest? request, out string errorMessage)
    {
        if (request == null)
        {
            errorMessage = "Request from notation was null";
            return false;
        }

        if (!ValidateContractVersion(request.ContractVersion, out errorMessage))
        {
            return false;
        }

        if (!ValidateKeyId(request.KeyId, out errorMessage))
        {
            return false;
        }

        return ValidatePluginConfig(request.PluginConfig, out errorMessage);
    }

    public static bool ValidateSignatureRequest([NotNullWhen(true)] SignatureRequest? request, out string errorMessage)
    {
        if (request == null)
        {
            errorMessage = "Request from notation was null";
            return false;
        }

        if (!ValidateContractVersion(request.ContractVersion, out errorMessage))
        {
            return false;
        }

        if (!ValidateKeyId(request.KeyId, out errorMessage))
        {
            return false;
        }

        if (!ValidateKeySpec(request.KeySpec, out errorMessage))
        {
            return false;
        }

        if (!ValidateHashAlgorithm(request.HashAlgorithm, out errorMessage))
        {
            return false;
        }

        if (!ValidatePayload(request.Payload, out errorMessage))
        {
            return false;
        }

        return ValidatePluginConfig(request.PluginConfig, out errorMessage);
    }

    private static bool ValidateContractVersion(string? contractVersion, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrEmpty(contractVersion))
        {
            errorMessage = "Request from notation was malformed: field contractVersion was null or empty";
            return false;
        }

        if (!PluginConstants.SupportedContractVersions.Contains(contractVersion))
        {
            errorMessage = $"Unsupported contract version from notation: {PluginConstants.Name} plugin does not support contract version {contractVersion}";
            return false;
        }

        return true;
    }

    private static bool ValidateKeyId(string? keyId, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrEmpty(keyId))
        {
            errorMessage = "Request from notation was malformed: field keyId was null or empty";
            return false;
        }

        return true;
    }

    private static bool ValidateKeySpec(string? keySpec, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrEmpty(keySpec))
        {
            errorMessage = "Request from notation was malformed: field keySpec was null or empty";
            return false;
        }

        if (PluginConstants.SupportedKeySpec != keySpec)
        {
            errorMessage = "Request from notation contains invalid or unsupported keySpec: " + keySpec;
            return false;
        }

        return true;
    }

    private static bool ValidateHashAlgorithm(string? hashAlgorithm, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrEmpty(hashAlgorithm))
        {
            errorMessage = "Request from notation was malformed: field hashAlgorithm was null or empty";
            return false;
        }

        // Currently TrustedSigning only supports RSA-3072 keys, which uses SHA-384 hashing.
        if (hashAlgorithm.Trim() != PluginConstants.SupportedHashAlgo)
        {
            errorMessage = "Request from notation contains invalid or unsupported hashAlgorithm: " + hashAlgorithm;
            return false;
        }

        return true;
    }

    private static bool ValidatePayload(string? payload, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrEmpty(payload))
        {
            errorMessage = "Request from notation was malformed: field payload was null or empty";
            return false;
        }

        return true;
    }

    private static bool ValidatePluginConfig([NotNullWhen(true)] PluginConfig? pluginConfig, out string errorMessage)
    {
        errorMessage = "";

        if (pluginConfig == null)
        {
            errorMessage = "pluginConfig from notation is null or empty";
            return false;
        }

        // We only validate the existence of these properties, not whether they are accurate or not.
        // We rely on TrustedSigning service to return an error if the credentials are invalid
        if (string.IsNullOrEmpty(pluginConfig.AccountName))
        {
            errorMessage = "accountName property is missing from pluginConfig, or its value is not provided";
            return false;
        }

        if (string.IsNullOrEmpty(pluginConfig.BaseUrl))
        {
            errorMessage = "baseUrl property is missing from pluginConfig, or its value is not provided";
            return false;
        }

        if (!Uri.IsWellFormedUriString(pluginConfig.BaseUrl, UriKind.Absolute))
        {
            errorMessage = "provided baseUrl is not a valid URL";
            return false;
        }

        if (string.IsNullOrEmpty(pluginConfig.CertProfile))
        {
            errorMessage = "certProfile property is missing from pluginConfig, or its value is not provided";
            return false;
        }

        return true;
    }
}
