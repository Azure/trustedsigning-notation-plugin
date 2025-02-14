// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

internal class PluginConfig
{
    public required string AccountName { get; set; }
    public required string CertProfile { get; set; }
    public required string BaseUrl { get; set; }
    public string? ExcludeCredentials { get; set; }
}
