// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin;

internal static class Metadata
{
    public static BaseResponse GenerateMetadataResponse(MetadataRequest? request)
    {
        if (!Validator.ValidateMetadataRequest(request, out string errorMessage))
        {
            throw new NotationException(errorMessage, ErrorCodes.ValidationError);
        }

        return new MetadataResponse()
        {
            Name = PluginConstants.Name,
            Description = PluginConstants.Description,
            Version = PluginConstants.PluginVersion,
            Url = PluginConstants.Url,
            SupportedContractVersions = PluginConstants.SupportedContractVersions,
            Capabilities = PluginConstants.Capabilities
        };
    }
}
