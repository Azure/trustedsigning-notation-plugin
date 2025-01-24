// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.TrustedSigning.NotationPlugin.UnitTests.Mocks;
using Azure.Developer.TrustedSigning.NotationPlugin.Utilities;

namespace Azure.Developer.TrustedSigning.NotationPlugin.UnitTests;

public class PluginUsageTests : TestBase, IDisposable
{
    [Fact]
    public void MainMethod_OutputsSuccess_ForValidMetadataRequest()
    {
        // Arrange
        string[] args = ["get-plugin-metadata"];
        var (outWriter, errorWriter) = SetupConsoleRedirection(ValidMetadataRequest);

        // Act
        Program.Main(args);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdError);
        Assert.NotEmpty(stdOut);
        Assert.Contains(PluginConstants.Description, stdOut);
    }

    [Fact]
    public void MainMethod_OutputsException_ForValidSignRequestWithContextException()
    {
        // Arrange
        string[] args = ["generate-signature"];
        var (outWriter, errorWriter) = SetupConsoleRedirection(ValidSignatureRequest);

        // Act
        int output = Program.Main(args);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdOut);
        Assert.NotEmpty(stdError);
        Assert.Contains(ErrorCodes.GenericError, stdError);
        Assert.Equal(1, output);
    }

    [Fact]
    public void MainMethod_OutputsCorrectError_ForInvalidRequest()
    {
        // Arrange
        string[] args = ["describe-key"];
        var (outWriter, errorWriter) = SetupConsoleRedirection(EmptyRequest);

        // Act
        Program.Main(args);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdOut);
        Assert.NotEmpty(stdError);
        Assert.Contains(ErrorCodes.ValidationError, stdError);
    }

    [Fact]
    public void MainMethod_OutputsCorrectError_ForInvalidVerb()
    {
        // Arrange
        string[] args = ["fake-verb"];
        const string expectedError = "Invalid/unsupported command was provided to plugin";
        var (outWriter, errorWriter) = SetupConsoleRedirection();

        // Act
        Program.Main(args);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdOut);
        Assert.NotEmpty(stdError);
        Assert.Contains(expectedError, stdError);
    }

    [Fact]
    public void MainMethod_OutputsCorrectError_ForMissingVerb()
    {
        // Arrange
        const string expectedError = "No command was provided to plugin.";
        var (outWriter, errorWriter) = SetupConsoleRedirection();

        // Act
        Program.Main([]);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdOut);
        Assert.NotEmpty(stdError);
        Assert.Contains(expectedError, stdError);
    }

    [Fact]
    public void ProcessCommand_OutputsSuccess_ForValidMetadataRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(ValidMetadataRequest);
        var (outWriter, errorWriter) = SetupConsoleRedirection();

        // Act
        Program.ProcessCommand(PluginConstants.MetadataCommand, TestSerializerOpts, provider);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdError);
        Assert.NotEmpty(stdOut);
        Assert.Contains(PluginConstants.Name, stdOut);
    }

    [Fact]
    public void ProcessCommand_ThrowsError_ForInValidMetadataRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get("{");

        // Act
        var exception = Assert.Throws<NotationException>(() => Program.ProcessCommand(PluginConstants.MetadataCommand, TestSerializerOpts, provider));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void ProcessCommand_OutputsSuccess_ForValidKeyRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(ValidKeyRequest);
        var (outWriter, errorWriter) = SetupConsoleRedirection();

        // Act
        Program.ProcessCommand(PluginConstants.KeyCommand, TestSerializerOpts, provider);

        // Assert
        string stdOut = outWriter.ToString();
        string stdError = errorWriter.ToString();

        Assert.Empty(stdError);
        Assert.NotEmpty(stdOut);
        Assert.Contains(PluginConstants.SupportedKeySpec, stdOut);
    }

    [Fact]
    public void ProcessCommand_OutputsError_ForInValidKeyRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(EmptyRequest);

        // Act
        var exception = Assert.Throws<NotationException>(() => Program.ProcessCommand(PluginConstants.KeyCommand, TestSerializerOpts, provider));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void ProcessCommand_OutputsError_ForInValidSignatureRequest()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(EmptyRequest);

        // Act
        var exception = Assert.Throws<NotationException>(() => Program.ProcessCommand(PluginConstants.SignatureCommand, TestSerializerOpts, provider));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.ValidationError, exception.ErrorCode);
    }

    [Fact]
    public void ProcessCommand_ThrowsException_ForDefaultSwitchCase()
    {
        // Arrange
        InputProvider provider = MockInputProvider.Get(EmptyRequest);
        const string expectedError = "Invalid/unsupported command was provided to plugin";

        // Act
        var exception = Assert.Throws<NotationException>(() => Program.ProcessCommand("fake-command", TestSerializerOpts, provider));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.GenericError, exception.ErrorCode);
        Assert.Contains(expectedError, exception.Message);
    }

    private static (StringWriter outWriter, StringWriter errorWriter) SetupConsoleRedirection(string? testUserInput = null)
    {
        // Conditionally set up the Console.In to read the user-provided string
        if (testUserInput is not null)
        {
            StringReader inputReader = new(testUserInput);
            Console.SetIn(inputReader);
        }

        // Redirect stdout and stderr to object that we can read
        StringWriter outWriter = new();
        StringWriter errorWriter = new();
        Console.SetOut(outWriter);
        Console.SetError(errorWriter);

        return (outWriter, errorWriter);
    }

    public void Dispose()
    {
        // Reset console properties
        StreamWriter stdOutStream = new(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        StreamWriter stdErrStream = new(Console.OpenStandardError())
        {
            AutoFlush = true
        };
        StreamReader stdInStream = new(Console.OpenStandardInput());

        Console.SetIn(stdInStream);
        Console.SetOut(stdOutStream);
        Console.SetError(stdErrStream);
        GC.SuppressFinalize(this);
    }
}
