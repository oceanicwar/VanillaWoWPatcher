namespace VanillaWoWPatcher.Utilities;

internal class ConsoleHelper
{
    public static void PrintColor(string message, ConsoleColor color)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        Console.WriteLine(message);

        Console.ForegroundColor = prevColor;
    }
}
