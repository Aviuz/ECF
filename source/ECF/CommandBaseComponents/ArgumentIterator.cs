namespace ECF.CommandBaseComponents
{
    internal class ArgumentIterator
    {
        private readonly string[] tokens;

        public int CurrentIndex { get; private set; } = 0;
        public int CurrentOrderedIndex { get; private set; } = 0;

        public ArgumentIterator(string[] tokens)
        {
            this.tokens = tokens;
        }

        public string? Peek(int index)
        {
            if (CurrentIndex + index < tokens.Length)
                return tokens[CurrentIndex + index];
            else
                return null;
        }

        public string? Take(bool ignoreOrder)
        {
            if (ignoreOrder == false)
                CurrentOrderedIndex++;

            if (CurrentIndex < tokens.Length)
                return tokens[CurrentIndex++];
            else
                return null;
        }
    }
}
