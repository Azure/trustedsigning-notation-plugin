// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography.X509Certificates;
using Azure.Developer.TrustedSigning.CryptoProvider.Interfaces;
using Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class SignTests : TestBase
{
    [Fact]
    public static void GenerateSignatureResponse_ReturnsErrorResponse_WithInvalidInput()
    {
        // Arrange
        SignatureRequest? request = null;

        // Act
        var exception = Assert.Throws<NotationException>(() => Sign.GenerateSignatureResponse(request));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public static void GenerateSignatureResponse_Succeeds_ReturnsVerifiableSignature()
    {
        // Arrange
        ISignContext context = MockSignContext.Get();
        SignatureRequest request = MockSignatureRequest.Get(context);

        // Act
        BaseResponse? response = Sign.GenerateSignatureResponse(request);

        // Assert
        var signResponse = (SignatureResponse) response;
        Assert.NotNull(signResponse);
        Assert.NotNull(signResponse.Signature);
        Assert.NotNull(signResponse.KeyId);
        Assert.NotNull(signResponse.SigningAlgorithm);
        Assert.NotNull(signResponse.CertificateChain);

        Assert.Equal(request.KeyId, signResponse.KeyId);
        Assert.Equal(PluginConstants.SupportedSignatureAlgo, signResponse.SigningAlgorithm);
        Assert.Equal(MockSignContext.ExpectedSignature, Convert.FromBase64String(signResponse.Signature));
    }

    [Fact]
    public static void GetSignature_ThrowsException_WithInvalidHashAlgo()
    {
        // Arrange
        ISignContext context = MockSignContext.Get();
        SignatureRequest request = GetDefaultSignatureRequest();
        request.HashAlgorithm = "SHA1";

        // Act
        var exception = Assert.Throws<NotationException>(() => Sign.GetSignature(request, context));

        // Assert
        Assert.NotNull(exception);
        Assert.Contains("Invalid or unsupported hash algo from notation", exception.Message);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public static void GetSignature_ThrowsException_WithInvalidPayload()
    {
        // Arrange
        ISignContext context = MockSignContext.Get();
        SignatureRequest request = GetDefaultSignatureRequest();
        request.Payload = "badbase64=====";

        // Act
        var exception = Assert.Throws<FormatException>(() => Sign.GetSignature(request, context));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public static void GetSignature_ThrowsException_OnCryptoProviderError()
    {
        // Arrange
        ISignContext context = MockSignContext.Get(signingFails: true);
        SignatureRequest request = GetDefaultSignatureRequest();

        // Act
        var exception = Assert.Throws<ApplicationException>(() => Sign.GetSignature(request, context));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(MockSignContext.ExpectedError, exception.Message);
    }

    [Fact]
    public static void GetSignature_Succeeds_ReturnsExpectedSignature()
    {
        // Arrange
        ISignContext context = MockSignContext.Get();
        SignatureRequest request = GetDefaultSignatureRequest();
        string expectedSig = Convert.ToBase64String(MockSignContext.ExpectedSignature);

        // Act
        string b64EncodedSignature = Sign.GetSignature(request, context);

        // Assert
        Assert.Equal(expectedSig, b64EncodedSignature);
    }

    [Fact]
    public static void FormatCertChain_ThrowsException_OnCryptoProviderError()
    {
        // Arrange
        ISignContext context = MockSignContext.Get(certChainFails: true);

        // Act
        var exception = Assert.Throws<ApplicationException>(() => Sign.FormatCertChain(context));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(MockSignContext.ExpectedError, exception.Message);
    }

    [Fact]
    public static void FormatCertChain_Succeeds_ReturnsValidCertChain()
    {
        // Arrange
        ISignContext context = MockSignContext.Get();

        // Act
        string[] chain = Sign.FormatCertChain(context);

        // Assert
        X509Certificate2[] decodedChain = chain
                                            .Select(cert => new X509Certificate2(Convert.FromBase64String(cert)))
                                            .ToArray();
        // Make sure all certs are in correct order
        Assert.NotEmpty(decodedChain);
        Assert.Equal(MockCertificateChain.ChildCert, decodedChain[0].Subject);
        Assert.Equal(MockCertificateChain.RootCert, decodedChain[1].Subject);
    }
}
