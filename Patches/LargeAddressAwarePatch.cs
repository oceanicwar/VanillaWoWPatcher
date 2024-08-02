
namespace VanillaWoWPatcher.Patches;

internal class LargeAddressAwarePatch : IPatch
{
    /* Patches the client to allow handling of over 2GB addresses. 
       Credits to: https://github.com/brndd/vanilla-tweaks/blob/master/src/main.rs#L156 */

    public string Name => "Large Address Aware";

    private const uint offset = 0x126;
    private const int offsetLength = 2;

    private const ushort originalFlags = ((ushort)Characteristics.IMAGE_FILE_RELOCS_STRIPPED |
                                        (ushort)Characteristics.IMAGE_FILE_EXECUTABLE_IMAGE |
                                        (ushort)Characteristics.IMAGE_FILE_LINE_NUMS_STRIPPED |
                                        (ushort)Characteristics.IMAGE_FILE_LOCAL_SYMS_STRIPPED |
                                        (ushort)Characteristics.IMAGE_FILE_32BIT_MACHINE);

    private enum Characteristics
    {
        IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
        IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
        IMAGE_FILE_LINE_NUMS_STRIPPED = 0x0004,
        IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x0008,
        IMAGE_FILE_AGGRESSIVE_WS_TRIM = 0x0010,
        IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020,
        IMAGE_FILE_RESERVED_FUTURE = 0x0040,
        IMAGE_FILE_BYTES_REVERSED_LO = 0x0080,
        IMAGE_FILE_32BIT_MACHINE = 0x0100,
        IMAGE_FILE_DEBUG_STRIPPED = 0x0200,
        IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x0400,
        IMAGE_FILE_NET_RUN_FROM_SWAP = 0x0800,
        IMAGE_FILE_SYSTEM = 0x1000,
        IMAGE_FILE_DLL = 0x2000,
        IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000,
        IMAGE_FILE_BYTES_REVERSED_HI = 0x8000
    }

    public PatchResult IsPatched(FileStream fs)
    {
        try
        {
            fs.Position = offset;

            byte[] buffer = new byte[offsetLength];
            fs.ReadExactly(buffer, 0, offsetLength);

            var mask = BitConverter.ToUInt16(buffer);

            return new PatchResult(mask != originalFlags);
        }
        catch(Exception ex)
        {
            return new PatchResult(false, $"{ex.Message}: {ex.StackTrace}");
        }
    }

    public PatchResult Patch(FileStream fs)
    {
        try
        {
            fs.Position = offset;

            var newFlags = originalFlags | (ushort)Characteristics.IMAGE_FILE_LARGE_ADDRESS_AWARE;
            fs.Write(BitConverter.GetBytes(newFlags), 0, offsetLength);

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
            fs.Position = offset;

            fs.Write(BitConverter.GetBytes(originalFlags), 0, offsetLength);

            return new PatchResult(true);
        }
        catch (Exception ex)
        {
            return new PatchResult(false, $"{ex.Message}: {ex.StackTrace}");
        }
    }
}
