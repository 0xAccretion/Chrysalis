using System.Formats.Cbor;

namespace Chrysalis.Cbor;

public interface  ICborObject<T> where T : ICborObject<T>
{
    byte[] ToCbor();
    CborWriter ToCbor(CborWriter writer);
    T FromCbor(byte[] data);
    T FromCbor(CborReader reader);
}
