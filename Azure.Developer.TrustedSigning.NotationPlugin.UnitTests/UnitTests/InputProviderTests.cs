// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class InputProviderTests : TestBase
{
    private const string NoInputError = "There was no input to TrustedSigning plugin";

    [Fact]
    public void ReadJsonRequest_Returns_ValidInput()
    {
        // Arrange
        const string sampleInput = "hello, world!";
        InputProvider provider = MockInputProvider.Get(sampleInput);

        // Act
        string rawRequest = provider.ReadJsonRequest();

        // Assert
        Assert.NotNull(rawRequest);
        Assert.NotEmpty(rawRequest);
        Assert.Equal(sampleInput, rawRequest);
    }

    [Fact]
    public void ReadJsonRequest_ThrowsCorrectError_WhenNoInputProvided()
    {
        // Arrange
        const string sampleInput = "   ";
        InputProvider provider = MockInputProvider.Get(sampleInput);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.ReadJsonRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(NoInputError, exception.Message);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetMetadataRequest_Succeeds_ForValidRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(ValidMetadataRequest);

        // Act
        var request = provider.GetMetadataRequest();

        // Assert
        Assert.NotNull(request);
        Assert.IsType<MetadataRequest>(request);
    }

    [Fact]
    public void GetMetadataRequest_ThrowsException_ForInvalidJson()
    {
        // Arrange
        const string sampleInput = "{";
        InputProvider provider = MockInputProvider.Get(sampleInput);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetMetadataRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetKeyRequest_Succeeds_ForValidRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(ValidKeyRequest);

        // Act
        var request = provider.GetKeyRequest();

        // Assert
        Assert.NotNull(request);
        Assert.IsType<KeyRequest>(request);
    }

    [Fact]
    public void GetKeyRequest_ThrowsException_ForInvalidJson()
    {
        // Arrange
        const string sampleInput = "{";
        InputProvider provider = MockInputProvider.Get(sampleInput);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetKeyRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetKeyRequest_ThrowsException_ForMissingProperties()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(EmptyRequest);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetKeyRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetSignatureRequest_Succeeds_ForValidRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(ValidSignatureRequest);

        // Act
        var request = provider.GetSignatureRequest();

        // Assert
        Assert.NotNull(request);
        Assert.IsType<SignatureRequest>(request);
    }

    [Fact]
    public void GetSignatureRequest_ThrowsException_ForInvalidJson()
    {
        // Arrange
        const string sampleInput = "{";
        InputProvider provider = MockInputProvider.Get(sampleInput);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetSignatureRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetSignatureRequest_ThrowsException_ForMissingProperties()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(EmptyRequest);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetSignatureRequest());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void GetDeserializedRequest_ThrowsException_ForNullInput()
    {
        // Arrange
        const string JsonNull = "null";
        InputProvider provider = MockInputProvider.Get(EmptyRequest);

        // Act
        var exception = Assert.Throws<NotationException>(() => provider.GetDeserializedRequest<object>(JsonNull));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }
}
