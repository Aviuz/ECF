using ECF.Tests.Mocks.Commands;

namespace ECF.Tests;

public class SyntaxTests
{
    [Fact]
    public void nameless_arguments_produces_index_instead_of_keys()
    {
        NamelessArgumentsComamnd command = new();

        Assert.Equal("<0> [<1>]", command.GetSyntaxExpression());
    }

    [Fact]
    public void arguments_with_names_produces_names_in_syntax()
    {
        CommandsWithRequiredParams command = new();

        Assert.Equal("<required_argument> [<non_required_argument>] --required-param <value> [--non-required-param <value>]", command.GetSyntaxExpression());
    }

    [Fact]
    public void BindingCommand_gives_correct_syntax()
    {
        BindingCommand command = new();
        string expectedSyntax = "<Argument1> [<0>] <cmdArg-2> [<1>] <cmdArg-3> [<2>] [-cp1|--cmd-parameter-1 <value>] [-cp2|--cmd-parameter-2 <value>] [-cp3|--cmd-parameter-3 <value>] [-p1|--param1 <value>] [-p2|--param2 <value>] [p3 <value>] [-cf1|--cmd-flag-1] [-cf2|--cmd-flag-2] [-cf3|--cmd-flag-3] [-f1|--flag1] [-f2|--flag2] [f3]";
        Assert.Equal(expectedSyntax, command.GetSyntaxExpression());
    }
}
