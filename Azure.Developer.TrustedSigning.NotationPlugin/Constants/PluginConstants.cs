// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Constants;

internal static class PluginConstants
{
    // Supported algorithms/specs
    // NOTE: currently we (TrustedSigning) only support RSA-3072 keys, and notation only supports RSA with PSS.
    public const string SupportedKeySpec = "RSA-3072";

    public const string SupportedHashAlgo = "SHA-384";
    public const string SupportedSignatureAlgo = "RSASSA-PSS-SHA-384";

    // Internal plugin constants
    internal const string VersionDelimiter = "::";

    // Plugin metadata
    public const string Name = "azure-trustedsigning";

    public const string Description = "Sign OCI artifacts using the Trusted Signing Service";
    public static readonly string ClientVersionString = GetClientVersionString();
    public static readonly string PluginVersion = ClientVersionString.Split(VersionDelimiter)[1];
    public const string Url = "https://aka.ms/TrustedSigning";
    public static readonly string[] SupportedContractVersions = ["1.0"];
    public static readonly string[] Capabilities = ["SIGNATURE_GENERATOR.RAW"];

    // Command line verbs
    public const string MetadataCommand = "get-plugin-metadata";
    public const string KeyCommand = "describe-key";
    public const string SignatureCommand = "generate-signature";

    // Returns client version string in the following format:
    // assemblyName::majorVersion.minorVersion.buildVersion
    internal static string GetClientVersionString()
    {
        var assembly = Assembly.GetExecutingAssembly();
        AssemblyName assemblyName = assembly.GetName();
        var version = assemblyName.Version ?? new Version(0, 0, 0);

        return $"{assemblyName.Name}{VersionDelimiter}{version.ToString(3)}";
    }
}
