namespace Chrysalis.Cbor;

public class CborSerializer
{
    public static T Deserialize<T>(byte[] data) where T : ICborObject, new()
    {
        T obj = new();
        obj.FromCbor(data);
        return obj;
    }

    public static byte[] Serialize<T>(T obj) where T : ICborObject
    {
        return obj.ToCbor();
    }
}
