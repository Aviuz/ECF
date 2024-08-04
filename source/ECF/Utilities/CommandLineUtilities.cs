using System.Text;

namespace ECF.Utilities;

public static class CommandLineTokenizer
{
    public static string[] Tokenize(string? commandLine)
    {
        if (commandLine == null)
            return Array.Empty<string>();

        List<string> tokens = new List<string>();
        StringBuilder currentToken = new StringBuilder();
        bool inQuotes = false;
        bool escapeNext = false;

        for (int i = 0; i < commandLine.Length; i++)
        {
            char c = commandLine[i];

            if (escapeNext)
            {
                currentToken.Append(c);
                escapeNext = false;
            }
            else if (c == '\\' && i + 1 < commandLine.Length && commandLine[i + 1] == '\"') // escape \"
            {
                escapeNext = true;
            }
            else if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (char.IsWhiteSpace(c) && !inQuotes)
            {
                if (currentToken.Length > 0)
                {
                    tokens.Add(currentToken.ToString());
                    currentToken.Clear();
                }
            }
            else
            {
                currentToken.Append(c);
            }
        }

        if (currentToken.Length > 0)
        {
            tokens.Add(currentToken.ToString());
        }

        return tokens.ToArray();
    }
}
