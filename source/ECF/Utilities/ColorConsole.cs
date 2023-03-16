using System;

namespace ECF.Utilities
{
    public static class ColorConsole
    {
        public static void WriteLineYellow(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }

        public static void WriteLineRed(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }

        public static void WriteYellow(string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(value);
            Console.ForegroundColor = oldColor;
        }

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
