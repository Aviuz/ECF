namespace ECF.CommandBaseComponents
{
    internal class ValueDictionary
    {
        public Dictionary<string, string?> StringValues { get; } = new Dictionary<string, string?>();
        public Dictionary<string, bool> BoolValues { get; } = new Dictionary<string, bool>();
    }
}
