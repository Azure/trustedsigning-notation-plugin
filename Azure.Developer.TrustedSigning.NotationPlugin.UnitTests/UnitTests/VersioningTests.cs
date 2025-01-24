// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class VersioningTests
{
    [Fact]
    public void MetadataResponse_Includes_Valid_AssemblyVersion()
    {
        // Arrange
        MetadataRequest request = new();

        // Act
        BaseResponse response = Metadata.GenerateMetadataResponse(request);

        // Assert
        var metadataResponse = (MetadataResponse) response;
        Assert.NotNull(metadataResponse);
        Assert.Equal(PluginConstants.PluginVersion, metadataResponse.Version);
    }

    [Fact]
    public void ClientVersionString_HasExpected_Format()
    {
        // Arrange
        string clientVersion = PluginConstants.ClientVersionString;

        // Act
        string[] versionTokens = clientVersion.Split(PluginConstants.VersionDelimiter);

        // Assert
        Assert.Equal(2, versionTokens.Length);
        Assert.False(string.IsNullOrWhiteSpace(versionTokens[0]));
        Assert.False(string.IsNullOrWhiteSpace(versionTokens[1]));
        Assert.Equal(3, versionTokens[1].Split(".").Length); // Ensure version number is of form major.minor.build
    }
}
