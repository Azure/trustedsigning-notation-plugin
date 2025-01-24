// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public abstract class TestBase
{
    internal const string EmptyRequest = "{}";
    internal const string ValidMetadataRequest = EmptyRequest;
    internal const string ValidKeyRequest = """{"contractVersion":"1.0","keyId":"fakeKeyId","pluginConfig":{"accountName":"fakeAccount","certProfile":"fakeProfile","baseUrl":"https://testtest1234notationplugin.com"}}""";
    internal const string ValidSignatureRequest = """{"keySpec":"RSA-3072","hashAlgorithm":"SHA-384","payload":"SGVsbG8gV29ybGQ=","contractVersion":"1.0","keyId":"fakeId","pluginConfig":{"accountName":"fakeAccount","certProfile":"fakeProfile","baseUrl":"https://testtest1234notationplugin.com"}}""";

    internal static JsonSerializerOptions TestSerializerOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    internal static KeyRequest GetDefaultKeyRequest() => new()
    {
        ContractVersion = PluginConstants.SupportedContractVersions[0],
        KeyId = "fakeId",
        PluginConfig = new PluginConfig()
        {
            AccountName = "fakeAccount",
            CertProfile = "fakeProfile",
            BaseUrl = "https://testtest1234notationplugin.com"
        }
    };

    internal static SignatureRequest GetDefaultSignatureRequest() => new()
    {
        ContractVersion = PluginConstants.SupportedContractVersions[0],
        KeyId = "fakeId",
        KeySpec = "RSA-3072",
        HashAlgorithm = "SHA-384",
        Payload = "testtest",
        PluginConfig = new PluginConfig()
        {
            AccountName = "fakeAccount",
            CertProfile = "fakeProfile",
            BaseUrl = "https://testtest1234notationplugin.com"
        }
    };

    internal static ErrorResponse ParseErrorResponse(string rawRequest)
    {
        ErrorResponse? response = JsonSerializer.Deserialize<ErrorResponse>(rawRequest, TestSerializerOpts);

        return response ?? throw new Exception("Parsing ErrorResponse failed");
    }
}
