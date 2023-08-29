using System;

namespace ECF.Utilities
{
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

        [Obsolete("WriteLineYellow is obsolete. Use WriteLine(string, ConsoleColor) instead.")]
        public static void WriteLineYellow(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }

        [Obsolete("WriteLineRed is obsolete. Use WriteLine(string, ConsoleColor) instead.")]
        public static void WriteLineRed(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }

        [Obsolete("WriteYellow is obsolete. Use Write(string, ConsoleColor) instead.")]
        public static void WriteYellow(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(value);
            Console.ForegroundColor = oldColor;
        }

        [Obsolete("WriteWhiteOnGreen is obsolete. Use Write(string, ConsoleColor, ConsoleColor) instead.")]
        public static void WriteWhiteOnGreen(string value)
        {
            ConsoleColor oldFColor = Console.ForegroundColor;
            ConsoleColor oldBColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(value);
            Console.ForegroundColor = oldFColor;
            Console.BackgroundColor = oldBColor;
        }
    } 
}
