using VanillaWoWPatcher.Patches;
using VanillaWoWPatcher.Utilities;

namespace VanillaWoWPatcher;

internal class Program
{
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

        var patches = new List<IPatch>()
        {
            new TrustedDomainPatch("oceanicwar.com"),
            new LargeAddressAwarePatch()
        };

        File.Copy(file, @"./WoW-patched.exe");
        using var fs = new FileStream(@"./wow-patched.exe", FileMode.Open, FileAccess.ReadWrite);

        ConsoleHelper.PrintColor("Patching WoW.exe..", ConsoleColor.White);

        foreach(var patch in patches)
        {
            ConsoleHelper.PrintColor($"Running '{patch.Name}' patch..", ConsoleColor.White);

            if(patch.IsPatched(fs).Success)
            {
                ConsoleHelper.PrintColor($"WoW.exe is already patched with '{patch.Name}'.", ConsoleColor.Yellow);
                continue;
            }

            var result = patch.Patch(fs);
            if(!result.Success)
            {
                ConsoleHelper.PrintColor($"Failed to patch: {result.Reason}", ConsoleColor.Red);
                continue;
            }
        }

        ConsoleHelper.PrintColor("Done patching, press ENTER to exit.", ConsoleColor.White);
        Console.ReadLine();
    }
}
