namespace ECF.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            new ECFProgramBuilder()
                .UseDefaultCommands()
                .AddConfiguration()
                .Configure((ctx, services, _) =>
                {
                    ctx.Intro = $"This is example console application based on ECF v.{typeof(Program).Assembly.GetName().Version}\nType help to list available commands";
                    ctx.Prefix = ">";
                })
                .Run(args);
        }
    }
}
