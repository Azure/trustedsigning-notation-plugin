// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

internal class InputProvider(TextReader reader, JsonSerializerOptions? serializerOpts = null)
{
    internal MetadataRequest GetMetadataRequest()
    {
        string rawRequest = ReadJsonRequest();
        return GetDeserializedRequest<MetadataRequest>(rawRequest);
    }

    internal KeyRequest GetKeyRequest()
    {
        string rawRequest = ReadJsonRequest();
        return GetDeserializedRequest<KeyRequest>(rawRequest);
    }

    internal SignatureRequest GetSignatureRequest()
    {
        string rawRequest = ReadJsonRequest();
        return GetDeserializedRequest<SignatureRequest>(rawRequest);
    }

    // Generic method to try and read a request, catching any exceptions that arise
    internal T GetDeserializedRequest<T>(string rawRequest)
    {
        try
        {
            var result = JsonSerializer.Deserialize<T>(rawRequest, serializerOpts);

            return result ?? throw new NotationException("Unable to parse request from notation", ErrorCodes.ValidationError);
        }
        catch (Exception ex)
        {
            throw new NotationException(ex.Message, ErrorCodes.ValidationError);
        }
    }

    internal string ReadJsonRequest()
    {
        string? input;

        if (string.IsNullOrWhiteSpace(input = reader.ReadLine()))
        {
            throw new NotationException("There was no input to TrustedSigning plugin", ErrorCodes.ValidationError);
        }

        return input;
    }
}
