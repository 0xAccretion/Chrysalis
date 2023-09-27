using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

public abstract class ByteConvertibleCborBase : ByteConvertibleBase, ICborObject<ByteConvertibleCborBase>
{
    public ByteConvertibleCborBase(byte[] bytes) : base(bytes)
    {
    }

    public ByteConvertibleCborBase(string hexString) : base(hexString)
    {
    }

    public abstract ByteConvertibleCborBase FromCbor(byte[] data);

    public abstract ByteConvertibleCborBase FromCbor(CborReader reader);

    public abstract byte[] ToCbor();

    public abstract CborWriter ToCbor(CborWriter writer);

    public override int GetHashCode()
    {
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(ToCbor());
        return BitConverter.ToInt32(hashBytes.Take(4).ToArray());
    }
}
