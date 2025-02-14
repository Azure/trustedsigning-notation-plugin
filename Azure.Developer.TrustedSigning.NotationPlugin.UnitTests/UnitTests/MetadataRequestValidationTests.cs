// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class MetadataRequestValidationTests
{
    const string NULL_REQUEST_ERROR = "Request from notation was null";

    [Fact]
    public void NullRequestReturnsError()
    {
        // Arrange
        MetadataRequest? request = null;

        // Act
        bool result = Validator.ValidateMetadataRequest(request, out string errorMessage);

        // Assert
        Assert.False(result);
        Assert.Equal(NULL_REQUEST_ERROR, errorMessage);
    }

    [Fact]
    public void ValidRequestPassesValidation()
    {
        // Arrange
        MetadataRequest? request = new();

        // Act
        bool result = Validator.ValidateMetadataRequest(request, out string errorMessage);

        // Assert
        Assert.True(result);
        Assert.True(string.IsNullOrEmpty(errorMessage));
    }
}
