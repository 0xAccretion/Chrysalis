namespace Chrysalis.Cbor;

public interface ICborObject
{
    byte[] ToCbor();
    void FromCbor(byte[] data);
}
