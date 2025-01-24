// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;

internal static class MockCertificateChain
{
    public static string RootCert => "CN=RootCert";
    public static string ChildCert => "CN=ChildCert";

    public static List<X509Certificate2> Get()
    {
        List<X509Certificate2> certificateChain = [];

        // Generate root certificate (cert1)
        using var rootKey = RSA.Create(2048);
        var rootCert = MockCertificate.Get(RootCert, null, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        // Generate first child certificate (leafCert) signed by root certificate
        using var leafCertKey = RSA.Create(2048);
        var leafCert = MockCertificate.Get(ChildCert, rootCert, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1));

        certificateChain.Add(leafCert);
        certificateChain.Add(rootCert);

        return certificateChain;
    }
}
