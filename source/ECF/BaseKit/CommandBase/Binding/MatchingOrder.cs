namespace ECF.BaseKit.CommandBase.Binding;

// This simplifies managing orders in binders, and avoid making hard-coded numbers in the code.
internal enum MatchingOrder
{
    FlagsAndParameters = 1,
    FlagsAndParameters_Obsolete = 2,
    Arguments = 3,
    Arguments_Obsolete = 4,
    BaseKitBinders = 5, // only --help for now (if overriden by custom flag it will be ignored)
}
