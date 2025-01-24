// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#plugin-metadata
/// </summary>
internal class MetadataResponse : BaseResponse
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Version { get; set; }
    public string? Url { get; set; }
    public string[]? SupportedContractVersions { get; set; }
    public string[]? Capabilities { get; set; }
}
