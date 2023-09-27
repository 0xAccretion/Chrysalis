namespace Chrysalis.Cardano.Models;

public static class ByteConvertibleFactory
{
    public static T? FromHex<T>(string hex) where T : ByteConvertibleBase, new()
    {
        return (T?)Activator.CreateInstance(typeof(T), hex) ?? throw new ArgumentNullException(nameof(T), "The provided type could not be created.");
    }

    public static T? FromBytes<T>(byte[] bytes) where T : ByteConvertibleBase, new()
    {
        return (T?)Activator.CreateInstance(typeof(T), bytes) ?? throw new ArgumentNullException(nameof(T), "The provided type could not be created.");
    }
}