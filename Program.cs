using VanillaWoWPatcher.Patches;
using VanillaWoWPatcher.Utilities;

namespace VanillaWoWPatcher;

internal class Program
{
    static List<IPatch> patches = new List<IPatch>();

    static void Main(string[] args)
    {
        if(args.Length < 1)
        {
            ConsoleHelper.PrintColor("You need to pass in a path to wow.exe or drop it onto the executable to patch.", ConsoleColor.Red);
            return;
        }

        var file = args[0];
        if(!File.Exists(file))
        {
            return;
        }

        patches.Add(new TrustedDomainPatch("oceanicwar.com"));

        using var fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);

        ConsoleHelper.PrintColor("Patching WoW.exe..", ConsoleColor.White);

        foreach(var patch in patches)
        {
            ConsoleHelper.PrintColor($"Running '{patch.Name}' patch..", ConsoleColor.White);

            var result = patch.Patch(fs);
            if(!result.Success)
            {
                ConsoleHelper.PrintColor($"Failed to patch: {result.Reason}", ConsoleColor.Red);
                continue;
            }
        }

        ConsoleHelper.PrintColor("Done patching.", ConsoleColor.White);
    }
}
