using ECF.Utilities;

namespace ECF.Tests;

public static class HelperExtensions
{
    public static string[] Tokenize(this string? input) => CommandLineTokenizer.Tokenize(input);
    public static string RemoveNoise(this string? str) => str!.Replace("\r\n", "\n").Replace(new string(new char[] { (char)0xFEFF }), string.Empty);
}
