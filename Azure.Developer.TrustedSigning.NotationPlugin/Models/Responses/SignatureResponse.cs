// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#generate-signature
/// </summary>
internal class SignatureResponse : BaseResponse
{
    public string KeyId { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string SigningAlgorithm { get; set; } = string.Empty;
    public string[] CertificateChain { get; set; } = [];
}
