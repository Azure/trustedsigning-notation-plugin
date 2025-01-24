// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#describe-key
/// </summary>
internal class KeyResponse : BaseResponse
{
    public string? KeyId { get; set; }
    public string? KeySpec { get; set; }
}
