using ECF.Utilities;

namespace ECF.Tests;

public static class HelperExtensions
{
    public static string[] Tokenize(this string? input) => CommandLineTokenizer.Tokenize(input);
}
