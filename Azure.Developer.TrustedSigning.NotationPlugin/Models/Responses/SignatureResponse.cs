// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#generate-signature
/// </summary>
internal class SignatureResponse : BaseResponse
{
    public required string KeyId { get; set; }
    public required string Signature { get; set; }
    public required string SigningAlgorithm { get; set; }
    public required string[] CertificateChain { get; set; }
}
