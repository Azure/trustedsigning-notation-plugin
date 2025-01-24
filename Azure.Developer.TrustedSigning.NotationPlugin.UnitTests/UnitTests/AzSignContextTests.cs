// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.CryptoProvider;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class AzSignContextTests : TestBase
{
    [Fact]
    public void GetAzSignContext_Returns_AzSignContext()
    {
        // Arrange
        SignatureRequest signRequest = GetDefaultSignatureRequest();

        // Act
        var context = signRequest.GetAzSignContext();

        // Assert
        Assert.IsType<AzSignContext>(context);
    }

    [Fact]
    public void GetAzSignContext_Returns_AzSignContext_WhenExcludedCredentialsProvided()
    {
        // Arrange
        SignatureRequest signRequest = GetDefaultSignatureRequest();
        signRequest.PluginConfig!.ExcludeCredentials = $"{CredentialOptions.Environment}, {CredentialOptions.ManagedIdentity}";

        // Act
        var context = signRequest.GetAzSignContext();

        // Assert
        Assert.IsType<AzSignContext>(context);
    }

    [Fact]
    public void GetAzSignContext_ThrowsException_ForInvalidRequest()
    {
        // Arrange
        SignatureRequest signRequest = GetDefaultSignatureRequest();
        signRequest.PluginConfig = null;

        // Act
        var exception = Assert.Throws<NotationException>(() => signRequest.GetAzSignContext());

        // Assert
        Assert.NotNull(exception);
        Assert.Contains("pluginConfig from request is null", exception.Message);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetAzureCredential_ThrowsException_ForUnsupportedExcludeCredentialValue()
    {
        // Arrange
        const string fakeCredentialType = "InvalidCredentialType";
        SignatureRequest signRequest = GetDefaultSignatureRequest();
        signRequest.PluginConfig!.ExcludeCredentials = $"{CredentialOptions.Environment}, {fakeCredentialType}";

        // Act
        var exception = Assert.Throws<NotationException>(() => signRequest.GetAzSignContext());

        // Assert
        Assert.NotNull(exception);
        Assert.Contains(fakeCredentialType, exception.Message);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }
}
