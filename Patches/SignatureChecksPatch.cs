using VanillaWoWPatcher.Utilities;

namespace VanillaWoWPatcher.Patches;

internal class SignatureChecksPatch : IPatch
{
    /* Disables signature checks for the game files.
     * Without this you cannot modify the game files.
     * Credits to: stoneharry https://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/501200-repost-sig-md5-protection-remover.html */

    public string Name => "Signature Checks";

    // { Offset, (Original, Modified) }
    private Dictionary<int, (byte, byte)> data = new()
    {
        { 0x2f113a, (0x5f, 0xeb) },
        { 0x2f113b, (0x5e, 0x19) },

        { 0x2f1158, (0x01, 0x03) },

        { 0x2f11a7, (0x01, 0x03) },

        { 0x2f11f0, (0x5f, 0xeb) },
        { 0x2f11f1, (0x5e, 0xb2) }
    };

    public PatchResult IsPatched(FileStream fs)
    {
        try
        {
            foreach (var p in data)
            {
                fs.Position = p.Key;

                if(fs.ReadByte() != p.Value.Item1)
                {
                    return new PatchResult(true);
                }
            }

            return new PatchResult(false);
        }
        catch (Exception ex)
        {
            return new PatchResult(false, $"{ex.Message}: {ex.StackTrace}");
        }
    }

    public PatchResult Patch(FileStream fs)
    {
        try
        {
            foreach (var entry in data)
            {
                ConsoleHelper.PrintColor($"> Writing to address '0x{entry.Key:X2}' with value '0x{entry.Value.Item2:X2}'.", ConsoleColor.Gray);

                fs.Position = entry.Key;
                fs.WriteByte(entry.Value.Item2);

                fs.Flush();
            }

            return new PatchResult(true);
        }
        catch (Exception ex)
        {
            return new PatchResult(false, $"{ex.Message}: {ex.StackTrace}");
        }
    }

    public PatchResult Unpatch(FileStream fs)
    {
        try
        {
            foreach (var p in data)
            {
                fs.Position = p.Key;
                fs.WriteByte(p.Value.Item1);

                fs.Flush();
            }

            return new PatchResult(true);
        }
        catch (Exception ex)
        {
            return new PatchResult(false, $"{ex.Message}: {ex.StackTrace}");
        }
    }
}
