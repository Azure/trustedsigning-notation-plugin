// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.Constants;

namespace Azure.Developer.TrustedSigning.NotationPlugin.Models;

internal class NotationException : Exception
{
    internal string ErrorCode { get; set; }

    internal NotationException(string msg, string errorCode)
        : base(msg)
    {
        ErrorCode = errorCode;
    }

    public NotationException() : base()
    {
        ErrorCode = ErrorCodes.GenericError;
    }

    public NotationException(string? message) : base(message)
    {
        ErrorCode = ErrorCodes.GenericError;
    }

    public NotationException(string? message, Exception? innerException) : base(message, innerException)
    {
        ErrorCode = ErrorCodes.GenericError;
    }
}
