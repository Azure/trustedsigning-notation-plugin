// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Azure.Developer.TrustedSigning.CryptoProvider.Interfaces;
using Azure.Developer.TrustedSigning.NotationPlugin.Constants;
using Azure.Developer.TrustedSigning.NotationPlugin.Models;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin;

internal static class Sign
{
    internal static BaseResponse GenerateSignatureResponse(SignatureRequest? request)
    {
        if (!Validator.ValidateSignatureRequest(request, out string errorMessage))
        {
            throw new NotationException(errorMessage, ErrorCodes.ValidationError);
        }

        ISignContext context = request.GetAzSignContext();
        string signature = GetSignature(request, context);
        string[] certChain = FormatCertChain(context);

        return new SignatureResponse()
        {
            KeyId = request.KeyId,
            Signature = signature,
            SigningAlgorithm = PluginConstants.SupportedSignatureAlgo,
            CertificateChain = certChain
        };
    }

    internal static string GetSignature(SignatureRequest request, ISignContext context)
    {
        // Decide hash algorithm based on request parameter
        // Currently TrustedSigning only supports RSA-3072 keys, which use SHA-384 hashing.
        HashAlgorithm hasher;
        if (request.HashAlgorithm == PluginConstants.SupportedHashAlgo)
            hasher = SHA384.Create();
        else
            throw new NotationException("Invalid or unsupported hash algo from notation: " + request.HashAlgorithm, ErrorCodes.ValidationError);

        // Create digest from the request payload
        byte[] decodedPayload = Convert.FromBase64String(request.Payload!);
        byte[]? digest = hasher.ComputeHash(decodedPayload);

        // Submit digest for signing with TrustedSigning
        // Notation only supports PSS padding https://github.com/notaryproject/notaryproject/blob/main/specs/plugin-extensibility.md#generate-signature
        byte[] sig = context.SignDigest(digest, RSASignaturePadding.Pss);

        return Convert.ToBase64String(sig);
    }

    // Returns a base64-encoded list of X509 certificates, ordered leaf -> root
    internal static string[] FormatCertChain(ISignContext context) => context.GetCertChain()
            .Select(cert => Convert.ToBase64String(cert.Export(X509ContentType.Cert)))
            .ToArray();
}
