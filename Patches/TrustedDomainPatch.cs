namespace VanillaWoWPatcher.Patches;

internal class TrustedDomainPatch : IPatch
{
    /* This patches the 1.12.1 client to allow the specified domain to be trusted.
     * Without this you cannot open custom URLs from the client. */

    public string Name => "Trusted Domain";

    private const uint offset = 0x45CCC0;
    private const uint offsetLength = 21;

    private string domain;

    public TrustedDomainPatch(string domain)
    {
        this.domain = domain;
    }

    public PatchResult Patch(FileStream fs)
    {
        if (domain.Length > offsetLength)
        {
            return new PatchResult(false, $"Domain '{domain}' was too long. It must be between 1-{offsetLength} characters.");
        }

        fs.Position = offset;

        foreach (var c in domain)
        {
            fs.WriteByte((byte)c);
        }

        if (domain.Length < offsetLength)
        {
            var diff = offsetLength - domain.Length;
            for (int i = 0; i < diff; i++)
            {
                fs.WriteByte(0x00);
            }
        }

        return new PatchResult(true);
    }
}
