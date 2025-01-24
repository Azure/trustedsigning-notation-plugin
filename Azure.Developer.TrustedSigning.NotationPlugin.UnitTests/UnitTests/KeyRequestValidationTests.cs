// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class KeyRequestValidationTests : TestBase
{
    const string NULL_REQUEST_ERROR = "Request from notation was null";
    const string MISSING_CONTRACT_ERROR = "Request from notation was malformed: field contractVersion was null or empty";
    const string INCORRECT_CONTRACT_ERROR = $"Unsupported contract version from notation: {PluginConstants.Name} plugin does not support contract version ";
    const string MISSING_KEYID_ERROR = "Request from notation was malformed: field keyId was null or empty";
    const string MISSING_PLUGINCONFIG_ERROR = "pluginConfig from notation is null or empty";
    const string MISSING_ACCOUNTNAME_ERROR = "accountName property is missing from pluginConfig, or its value is not provided";
    const string MISSING_BASEURL_ERROR = "baseUrl property is missing from pluginConfig, or its value is not provided";
    const string MISSING_CERTPROFILE_ERROR = "certProfile property is missing from pluginConfig, or its value is not provided";
    const string INVALID_BASEURL_ERROR = "provided baseUrl is not a valid URL";

    [Fact]
    public void ValidRequestPassesValidation()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.True(result);
        Assert.True(string.IsNullOrEmpty(errorMessage));
    }

    [Fact]
    public void NullRequestReturnsError()
    {
        // Arrange
        KeyRequest? request = null;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(NULL_REQUEST_ERROR, errorMessage);
    }

    [Fact]
    public void MissingContractVersionReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.ContractVersion = null!;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

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

        KeyRequest request = GetDefaultKeyRequest();
        request.ContractVersion = fakeContractVersion;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedError, errorMessage);
    }

    [Fact]
    public void MissingKeyIdReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.KeyId = null!;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_KEYID_ERROR, errorMessage);
    }

    [Fact]
    public void NullPluginConfigReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.PluginConfig = null;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_PLUGINCONFIG_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_AccountName_ReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.PluginConfig!.AccountName = null!;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_ACCOUNTNAME_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_BaseUrl_ReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.PluginConfig!.BaseUrl = null!;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_BASEURL_ERROR, errorMessage);
    }

    [Fact]
    public void MissingPluginConfig_CertProfile_ReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.PluginConfig!.CertProfile = null!;

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(MISSING_CERTPROFILE_ERROR, errorMessage);
    }

    [Fact]
    public void InvalidPluginConfig_BaseUrl_ReturnsError()
    {
        // Arrange
        KeyRequest request = GetDefaultKeyRequest();
        request.PluginConfig!.BaseUrl = "fakeUrl";

        // Act
        bool result = Validator.ValidateKeyRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(INVALID_BASEURL_ERROR, errorMessage);
    }
}
