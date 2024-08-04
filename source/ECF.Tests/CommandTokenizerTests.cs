using ECF.Utilities;
using System.Runtime.InteropServices;

namespace ECF.Tests;

public class CommandTokenizerTests
{
    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData(" ", new string[0])]
    [InlineData("   ", new string[0])]
    [InlineData("test this command", new[] { "test", "this", "command" })]
    [InlineData("test this  command", new[] { "test", "this", "command" })]
    [InlineData("test \"this command\"", new[] { "test", "this command" })]
    [InlineData("test \"this \\\" command\"", new[] { "test", "this \" command" })]
    [InlineData("program-name --option1 value1 --option2 value2", new[] { "program-name", "--option1", "value1", "--option2", "value2" })]
    [InlineData("program-name --option1 \"value with spaces\" --option2 value2 positional1", new[] { "program-name", "--option1", "value with spaces", "--option2", "value2", "positional1" })]
    [InlineData("program-name --option1 value1 --option2 value2 --flag1", new[] { "program-name", "--option1", "value1", "--option2", "value2", "--flag1" })]
    [InlineData("program-name --help", new[] { "program-name", "--help" })]
    [InlineData("program-name --option1 'single quotes'", new[] { "program-name", "--option1", "'single", "quotes'" })]
    [InlineData("program-name --option1 value1 --option2 value2 --flag1 --flag2", new[] { "program-name", "--option1", "value1", "--option2", "value2", "--flag1", "--flag2" })]
    [InlineData("program-name --option1 value1 --option2 \"complex value with spaces and \\backslashes\"", new[] { "program-name", "--option1", "value1", "--option2", "complex value with spaces and \\backslashes" })]
    [InlineData("program-name --flag1 --flag2 --flag3 value1", new[] { "program-name", "--flag1", "--flag2", "--flag3", "value1" })]
    [InlineData("program-name  -- option2 value2 positional1 positional2", new[] { "program-name", "--", "option2", "value2", "positional1", "positional2" })]
    [InlineData("  program-name --option1 value1  ", new[] { "program-name", "--option1", "value1" })]
    [InlineData("  program-name \"35\"  ", new[] { "program-name", "35" })]
    public void tokenize_seperates_into_correct_parts(string? input, string[] expectedOutput)
    {
        string[] output = CommandLineTokenizer.Tokenize(input);

        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("test this command")]
    [InlineData("test this  command")]
    [InlineData("test \"this command\"")]
    [InlineData("test \"this \\\" command\"")]
    [InlineData("program-name --option1 value1 --option2 value2")]
    [InlineData("program-name --option1 \"value with spaces\" --option2 value2 positional1")]
    [InlineData("program-name --option1 value1 --option2 value2 --flag1")]
    [InlineData("program-name --help")]
    [InlineData("program-name --option1 'single quotes'")]
    [InlineData("program-name --option1 value1 --option2 value2 --flag1 --flag2")]
    [InlineData("program-name --option1 value1 --option2 \"complex value with spaces and \\backslashes\"")]
    [InlineData("program-name --flag1 --flag2 --flag3 value1")]
    [InlineData("program-name  -- option2 value2 positional1 positional2")]
    [InlineData("program-name \\\"35\\\"  ")]
    public void works_same_as_CommandLineToArgvW(string? input)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // this test does not work on linux
        {
            string[] outputFromTokenizer = CommandLineTokenizer.Tokenize(input);
            string[] outputFromCommandToArgsW = CommandLineUtilities.CommandLineToArgs(input);

            Assert.Equal(outputFromCommandToArgsW, outputFromTokenizer);
        }
    }

    internal static class CommandLineUtilities
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string? lpCmdLine, out int pNumArgs);

        internal static string[] CommandLineToArgs(string? commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                return Array.Empty<string>();

            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);

            if (argv == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                var args = new string[argc];
                for (int i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p)!;
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
