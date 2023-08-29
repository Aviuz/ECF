using ECF;

namespace Example.Commands
{
    [Command("test-binding", "tb")]
    [CmdArgument("arg1", 0, Name = "Arg 1", Description = "arg1 description")]
    [CmdParameter("param1", Description = "param1 desc", ShortName = "p", LongName = "param")]
    [CmdFlag("flag1", ShortName = "f", LongName = "flag", Description = "Flag description")]
    [CmdFlag("flag4", ShortName = "f4", Description = "Flag4 description")]
    [CmdArgument("arg3", 2, Name = "Arg 3", Description = "arg3 description")]
    internal class TestBinding : CommandBase
    {
        public string? Arg1 => GetValue("arg1");
        public string? Arg3 => GetValue("arg3");
        public bool Flag1 => IsFlagActive("flag1");
        public string? Param1 => GetValue("param1");

        [Argument(1, Description = "Arg2 Desciption", Name = "arg 2")]
        public string? Arg2 { get; set; }
        [Parameter(ShortName = "p2", LongName = "param2", Description = "Param2 description")]
        public string? Param2 { get; set; }
        [Flag(ShortName = "f2", LongName = "flag2", Description = "Flag2 description")]
        public bool Flag2 { get; set; }
        [Flag(ShortName = "f3", LongName = "flag3", Description = "Flag3 description")]
        public int Flag3 { get; set; }
        [Parameter(ShortName = "p3", LongName = "param3", Description = "Param as int")]
        public int Param3 { get; set; }

        public override void Execute()
        {
            if (Arg1 == null)
                DisplayHelp();

            Console.WriteLine($"Arg1: {Arg1}; Arg2: {Arg2}; Arg3: {Arg3}; Param1: {Param1}; Param2: {Param2}; Param3: {Param3}; Flag1: {Flag1}; Flag2: {Flag2}; Flag3: {Flag3}");
        }
    }
}
