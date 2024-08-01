namespace VanillaWoWPatcher.Patches;

internal interface IPatch
{
    string Name { get; }
    PatchResult Patch(FileStream fs);
}
