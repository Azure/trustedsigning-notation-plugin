// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;

internal static class MockInputProvider
{
    internal static InputProvider Get(string payload, JsonSerializerOptions? serializerOpts = null)
    {
        serializerOpts ??= new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        StringReader reader = new(payload);
        return new(reader, serializerOpts);
    }
}
