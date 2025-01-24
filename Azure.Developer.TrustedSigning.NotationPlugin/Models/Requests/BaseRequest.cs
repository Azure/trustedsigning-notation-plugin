// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

internal abstract class BaseRequest
{
    public PluginConfig? PluginConfig { get; set; }
}
