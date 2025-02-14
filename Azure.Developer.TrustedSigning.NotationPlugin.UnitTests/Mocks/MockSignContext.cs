// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Azure.Developer.TrustedSigning.CryptoProvider.Interfaces;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;

internal static class MockSignContext
{
    internal static readonly byte[] ExpectedSignature = [1, 2, 3, 4, 5];
    internal const string ExpectedError = "Error from CryptoProvider";
    internal static readonly List<X509Certificate2> ExpectedChain = MockCertificateChain.Get();

    public static ISignContext Get(bool signingFails = false, bool certChainFails = false)
    {
        var mockContext = new Mock<ISignContext>();

        byte[] signResult = [];
        var signDigestSetup = mockContext.Setup(m => m.SignDigest(It.IsAny<byte[]>(), It.IsAny<RSASignaturePadding>(), It.IsAny<X509Certificate2>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
        var certChainSetup = mockContext.Setup(m => m.GetCertChain(It.IsAny<CancellationToken>()));

        // Different setups based on whether we want SignDigest to fail or pass
        if (signingFails)
            signDigestSetup.Throws(new ApplicationException(ExpectedError));
        else
            signDigestSetup.Returns(ExpectedSignature);

        // Different setups based on whether we want FormatCertChain to fail or pass
        if (certChainFails)
            certChainSetup.Throws(new ApplicationException(ExpectedError));
        else
            certChainSetup.Returns(ExpectedChain);

        return mockContext.Object;
    }
}
