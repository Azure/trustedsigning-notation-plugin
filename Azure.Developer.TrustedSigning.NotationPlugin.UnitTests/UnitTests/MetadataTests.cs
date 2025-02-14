// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class MetadataTests
{
    [Fact]
    public void InvalidRequest_Returns_ErrorResponse()
    {
        // Arrange
        MetadataRequest? request = null;

        // Act
        var exception = Assert.Throws<NotationException>(() => Metadata.GenerateMetadataResponse(request));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void ValidRequest_Returns_SuccessResponse()
    {
        // Arrange
        MetadataRequest request = new();

        // Act
        BaseResponse response = Metadata.GenerateMetadataResponse(request);

        // Assert
        var metadataResponse = (MetadataResponse) response;
        Assert.NotNull(metadataResponse);
        Assert.Equal(PluginConstants.Name, metadataResponse.Name);
        Assert.Equal(PluginConstants.Description, metadataResponse.Description);
        Assert.Equal(PluginConstants.PluginVersion, metadataResponse.Version);
        Assert.Equal(PluginConstants.Url, metadataResponse.Url);
        Assert.Equal(PluginConstants.SupportedContractVersions, metadataResponse.SupportedContractVersions);
        Assert.Equal(PluginConstants.Capabilities, metadataResponse.Capabilities);
    }
}
