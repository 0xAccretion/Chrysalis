using System.Formats.Cbor;

namespace Chrysalis.Cbor;

public interface ICborObject
{
    byte[] ToCbor();
    void FromCbor(byte[] data);
    void FromCbor(CborReader reader);
}
