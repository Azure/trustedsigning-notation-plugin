// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#plugin-metadata
/// </summary>
internal class MetadataResponse : BaseResponse
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Version { get; set; }
    public required string Url { get; set; }
    public required string[] SupportedContractVersions { get; set; }
    public required string[] Capabilities { get; set; }
}
