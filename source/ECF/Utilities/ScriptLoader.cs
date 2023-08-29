using ECF.Engine;

namespace ECF.Utilities;

public class ScriptLoader
{
    private readonly InterfaceContext context;

    public ScriptLoader(InterfaceContext context)
    {
        this.context = context;
    }

    public void Load(TextReader textReader)
    {
        if (context?.CommandProcessor == null)
            throw new Exception("Cannot load script if CommandProcesor isn't initialized. InterfaceContext need to have CommandScope with processor attached.");

        string? line;
        do
        {
            line = textReader.ReadLine()?.Trim();

            // Empty line
            if (string.IsNullOrEmpty(line))
                continue;

            // Comment line
            if (line.StartsWith("//"))
                continue;

            context.CommandProcessor.Process(line);
        }
        while (line != null && !context.ForceExit);
    }
}
