// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin;

internal static class Program
{
    internal static int Main(string[] args)
    {
        JsonSerializerOptions serializerOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        InputProvider inputProvider = new(Console.In, serializerOpts);

        try
        {
            if (args.Length == 0)
            {
                throw new NotationException("No command was provided to plugin.", ErrorCodes.GenericError);
            }

            ProcessCommand(args[0], serializerOpts, inputProvider);

            return 0;
        }
        catch (NotationException ex)
        {
            // These errors are intentionally thrown by us and have an ErrorCode property
            ErrorResponse response = new() { ErrorCode = ex.ErrorCode, ErrorMessage = ex.Message };
            Console.Error.WriteLine(JsonSerializer.Serialize<object>(response, serializerOpts));

            return 1;
        }
        catch (Exception ex)
        {
            // If an error is caught here, it was unexpected
            ErrorResponse response = new() { ErrorCode = ErrorCodes.GenericError, ErrorMessage = ex.Message };
            Console.Error.WriteLine(JsonSerializer.Serialize<object>(response, serializerOpts));

            return 1;
        }
    }

    // Process the incoming request and write a response to StdOut
    internal static void ProcessCommand(string command, JsonSerializerOptions serializerOpts, InputProvider inputProvider)
    {
        BaseResponse response;

        switch (command)
        {
            case PluginConstants.MetadataCommand:
                MetadataRequest metaReq = inputProvider.GetMetadataRequest();
                response = Metadata.GenerateMetadataResponse(metaReq);
                break;
            case PluginConstants.KeyCommand:
                KeyRequest keyReq = inputProvider.GetKeyRequest();
                response = Key.GenerateKeyResponse(keyReq);
                break;
            case PluginConstants.SignatureCommand:
                SignatureRequest sigReq = inputProvider.GetSignatureRequest();
                response = Sign.GenerateSignatureResponse(sigReq);
                break;
            default:
                throw new NotationException($"Invalid/unsupported command was provided to plugin: {command}", ErrorCodes.GenericError);
        }

        Console.WriteLine(JsonSerializer.Serialize<object>(response, serializerOpts));
    }
}
