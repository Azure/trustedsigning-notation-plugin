// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class SignRequestValidationTests : TestBase
{
    const string NULL_REQUEST_ERROR = "Request from notation was null";
    const string MISSING_CONTRACT_ERROR = "Request from notation was malformed: field contractVersion was null or empty";
    const string INCORRECT_CONTRACT_ERROR = $"Unsupported contract version from notation: {PluginConstants.Name} plugin does not support contract version ";
    const string MISSING_KEYID_ERROR = "Request from notation was malformed: field keyId was null or empty";
    const string MISSING_KEYSPEC_ERROR = "Request from notation was malformed: field keySpec was null or empty";
    const string INCORRECT_KEYSPEC_ERROR = "Request from notation contains invalid or unsupported keySpec: ";
    const string MISSING_HASHALGORITHM_ERROR = "Request from notation was malformed: field hashAlgorithm was null or empty";
    const string INCORRECT_HASHALGORITHM_ERROR = "Request from notation contains invalid or unsupported hashAlgorithm: ";
    const string MISSING_PAYLOAD_ERROR = "Request from notation was malformed: field payload was null or empty";
    const string MISSING_PLUGINCONFIG_ERROR = "pluginConfig from notation is null or empty";
    const string MISSING_ACCOUNTNAME_ERROR = "accountName property is missing from pluginConfig, or its value is not provided";
    const string MISSING_BASEURL_ERROR = "baseUrl property is missing from pluginConfig, or its value is not provided";
    const string MISSING_CERTPROFILE_ERROR = "certProfile property is missing from pluginConfig, or its value is not provided";
    const string INVALID_BASEURL_ERROR = "provided baseUrl is not a valid URL";

    [Fact]
    public void ValidRequestPassesValidation()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.True(result);
        Assert.True(string.IsNullOrEmpty(errorMessage));
    }

    [Fact]
    public void NullRequestReturnsError()
    {
        // Arrange
        SignatureRequest? request = null;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(NULL_REQUEST_ERROR, errorMessage);
    }

    [Fact]
    public void MissingContractVersionReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.ContractVersion = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_CONTRACT_ERROR, errorMessage);
    }

    [Fact]
    public void IncorrectContractVersionReturnsError()
    {
        // Arrange
        const string fakeContractVersion = "-1";
        const string expectedError = INCORRECT_CONTRACT_ERROR + fakeContractVersion;

        SignatureRequest? request = GetDefaultSignatureRequest();
        request.ContractVersion = fakeContractVersion;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedError, errorMessage);
    }

    [Fact]
    public void MissingKeyIdReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.KeyId = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_KEYID_ERROR, errorMessage);
    }

    [Fact]
    public void MissingKeySpecReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.KeySpec = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_KEYSPEC_ERROR, errorMessage);
    }

    [Fact]
    public void IncorrectKeySpecReturnsError()
    {
        // Arrange
        const string fakeKeySpec = "AES-256";
        const string expectedError = INCORRECT_KEYSPEC_ERROR + fakeKeySpec;

        SignatureRequest? request = GetDefaultSignatureRequest();
        request.KeySpec = fakeKeySpec;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedError, errorMessage);
    }

    [Fact]
    public void MissingHashAlgorithmReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.HashAlgorithm = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_HASHALGORITHM_ERROR, errorMessage);
    }

    [Fact]
    public void IncorrectHashAlgorithmReturnsError()
    {
        // Arrange
        const string fakeHashAlgorithm = "SHA-256";
        const string expectedError = INCORRECT_HASHALGORITHM_ERROR + fakeHashAlgorithm;

        SignatureRequest? request = GetDefaultSignatureRequest();
        request.HashAlgorithm = fakeHashAlgorithm;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedError, errorMessage);
    }

    [Fact]
    public void MissingPayloadReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.Payload = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_PAYLOAD_ERROR, errorMessage);
    }

    [Fact]
    public void NullPluginConfigReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.PluginConfig = null;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_PLUGINCONFIG_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_AccountName_ReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.PluginConfig!.AccountName = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_ACCOUNTNAME_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_CertProfile_ReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.PluginConfig!.CertProfile = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_CERTPROFILE_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_BaseUrl_ReturnsError()
    {
        // Arrange
        SignatureRequest? request = GetDefaultSignatureRequest();
        request.PluginConfig!.BaseUrl = null!;

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_BASEURL_ERROR, errorMessage);
    }

    [Fact]
    public void InvalidPluginConfig_BaseUrl_ReturnsError()
    {
        // Arrange
        SignatureRequest request = GetDefaultSignatureRequest();
        request.PluginConfig!.BaseUrl = "fakeUrl";

        // Act
        bool result = Validator.ValidateSignatureRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(INVALID_BASEURL_ERROR, errorMessage);
    }
}
