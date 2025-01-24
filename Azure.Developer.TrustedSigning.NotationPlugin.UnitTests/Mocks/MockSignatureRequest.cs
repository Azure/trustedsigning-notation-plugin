// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.CryptoProvider.Interfaces;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

internal static class MockSignatureRequest
{
    public static SignatureRequest Get(ISignContext context)
    {
        var mockSignatureRequest = new Mock<SignatureRequest>();
        var validRequest = TestBase.GetDefaultSignatureRequest();

        mockSignatureRequest.SetupAllProperties();
        mockSignatureRequest.Object.ContractVersion = validRequest.ContractVersion;
        mockSignatureRequest.Object.KeyId = validRequest.KeyId;
        mockSignatureRequest.Object.KeySpec = validRequest.KeySpec;
        mockSignatureRequest.Object.HashAlgorithm = validRequest.HashAlgorithm;
        mockSignatureRequest.Object.Payload = validRequest.Payload;
        mockSignatureRequest.Object.PluginConfig = validRequest.PluginConfig;

        mockSignatureRequest.Setup(req => req.GetAzSignContext()).Returns(context);

        return mockSignatureRequest.Object;
    }
}
