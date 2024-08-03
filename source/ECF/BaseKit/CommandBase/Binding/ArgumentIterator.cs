namespace ECF.BaseKit.CommandBase.Binding;

internal class ArgumentIterator
{
    private readonly string[] tokens;

    public int CurrentIndex { get; private set; } = 0;
    public int OrderedArgumentsConsumedCount { get; private set; } = 0;

    public ArgumentIterator(string[] tokens)
    {
        this.tokens = tokens;
    }

    /// <summary>
    /// Get token at specified index.
    /// </summary>
    /// <param name="deltaIndex">delta relative to current index</param>
    public string? Get(int deltaIndex = 0)
    {
        if (CurrentIndex + deltaIndex < tokens.Length && CurrentIndex + deltaIndex >= 0)
            return tokens[CurrentIndex + deltaIndex];
        else
            return null;
    }

    /// <summary>
    /// Advance reader with specified token count.
    /// </summary>
    public void Advance(int tokenCount = 1)
    {
        CurrentIndex += tokenCount;
    }

    /// <summary>
    /// Advance reader with specified token count and advance argument count. This is used for ordered arguments.
    /// </summary>
    public void AdvanceWithArgument(int tokenCount = 1)
    {
        CurrentIndex += tokenCount;
        OrderedArgumentsConsumedCount += tokenCount;
    }
}
