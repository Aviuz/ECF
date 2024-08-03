using ECF.Engine;

namespace ECF.Utilities;

public class ScriptLoader
{
    private readonly InterfaceContext context;

    public ScriptLoader(InterfaceContext context)
    {
        this.context = context;
    }

    public async Task LoadAsync(TextReader textReader, CancellationToken cancellationToken = default)
    {
        if (context?.CommandProcessor == null)
            throw new Exception("Cannot load script if CommandProcesor isn't initialized. InterfaceContext need to have CommandScope with processor attached.");

        string? line;
        do
        {
            line = (await textReader.ReadLineAsync())?.Trim();

            // Empty line
            if (string.IsNullOrEmpty(line))
                continue;

            // Comment line
            if (line.StartsWith("//"))
                continue;

            await context.CommandProcessor.ProcessAsync(line, cancellationToken);
        }
        while (line != null && !context.ForceExit);
    }
}
