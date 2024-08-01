namespace VanillaWoWPatcher.Patches;

internal class PatchResult
{
    public bool Success { get; set; }
    public string Reason { get; set; }

    public PatchResult(bool success, string reason = "")
    {
        this.Success = success;
        this.Reason = reason;
    }
}
