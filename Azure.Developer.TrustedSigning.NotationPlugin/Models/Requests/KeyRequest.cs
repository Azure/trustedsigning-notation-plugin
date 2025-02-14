// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#describe-key
/// </summary>
internal class KeyRequest : BaseRequest
{
    public required string ContractVersion { get; set; }
    public required string KeyId { get; set; }
}
