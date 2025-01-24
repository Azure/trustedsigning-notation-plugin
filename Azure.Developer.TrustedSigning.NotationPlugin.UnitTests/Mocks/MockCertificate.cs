// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;

internal static class MockCertificate
{
    const string SelfSigned = "CN=SelfSignedCert";

    public static X509Certificate2 Get(string subjectName = SelfSigned, X509Certificate2? issuer = null, DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        from ??= DateTimeOffset.UtcNow.AddYears(-1);
        to ??= DateTimeOffset.UtcNow.AddHours(1);

        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest(new X500DistinguishedName(subjectName), rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        // Add Basic Constraints extension
        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(issuer == null, issuer != null, 0, true));

        // Add Key Usage extension
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.DigitalSignature, true));

        if (issuer is null)
        {
            // Self-signed certificate
            return request.CreateSelfSigned(from.Value, to.Value);
        }

        // Certificate signed by issuer
        return request.Create(issuer, from.Value, to.Value, Guid.NewGuid().ToByteArray());
    }
}
