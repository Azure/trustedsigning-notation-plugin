// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class KeyTests : TestBase
{
    [Fact]
    public void InvalidRequest_Returns_ErrorResponse()
    {
        // Arrange
        KeyRequest? keyRequest = null;

        // Act
        var exception = Assert.Throws<NotationException>(() => Key.GenerateKeyResponse(keyRequest));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void ValidRequest_Returns_SuccessResponse()
    {
        // Arrange
        KeyRequest keyRequest = GetDefaultKeyRequest();

        // Act
        BaseResponse response = Key.GenerateKeyResponse(keyRequest);

        // Assert
        var keyResponse = (KeyResponse) response;
        Assert.NotNull(keyResponse);
        Assert.Equal(PluginConstants.SupportedKeySpec, keyResponse.KeySpec);
        Assert.Equal(keyRequest.KeyId, keyResponse.KeyId);
    }
}
