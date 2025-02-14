// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin;

internal static class Key
{
    public static BaseResponse GenerateKeyResponse(KeyRequest? request)
    {
        if (!Validator.ValidateKeyRequest(request, out string errorMessage))
        {
            throw new NotationException(errorMessage, ErrorCodes.ValidationError);
        }

        // Currently, TrustedSigning only supports 3072 bit RSA keys. We will always return RSA-3072 as keyspec until TrustedSigning expands its offerings
        return new KeyResponse()
        {
            KeyId = request.KeyId,
            KeySpec = PluginConstants.SupportedKeySpec
        };
    }
}
