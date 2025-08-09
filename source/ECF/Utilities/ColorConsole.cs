namespace ECF.Utilities;

public static class ColorConsole
{
    public static void Write(string value, ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(value);
        Console.ForegroundColor = oldColor;
    }

    public static void WriteLine(string value, ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ForegroundColor = oldColor;
    }

    public static void Write(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        ConsoleColor oldFColor = Console.ForegroundColor;
        ConsoleColor oldBColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write(value);
        Console.ForegroundColor = oldFColor;
        Console.BackgroundColor = oldBColor;
    }

    public static void WriteLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        ConsoleColor oldFColor = Console.ForegroundColor;
        ConsoleColor oldBColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.WriteLine(value);
        Console.ForegroundColor = oldFColor;
        Console.BackgroundColor = oldBColor;
    }
}
