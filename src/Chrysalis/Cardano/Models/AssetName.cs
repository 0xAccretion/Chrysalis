using System.Text;

namespace Chrysalis.Cardano.Models;


/// <summary>
/// Represents an AssetName, which is a ByteString that can be converted to ASCII.
/// </summary>
/// <remarks>
/// This class inherits from the ByteString class and adds a method to convert the stored bytes to ASCII.
/// </remarks>
public class AssetName : ByteString
{
    /// <summary>
    /// Initializes a new instance of the AssetName class with an empty byte array.
    /// </summary>
    public AssetName() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the AssetName class with the specified byte array.
    /// </summary>
    /// <param name="bytes">The byte array to initialize the AssetName with.</param>
    public AssetName(byte[] bytes) : base(bytes)
    {
    }

    /// <summary>
    /// Initializes a new instance of the AssetName class with the specified hex string.
    /// </summary>
    /// <param name="hex">The hex string to initialize the AssetName with.</param>
    public AssetName(string hex) : base(hex)
    {
    }

    /// <summary>
    /// Converts the stored bytes to an ASCII string.
    /// </summary>
    /// <returns>The ASCII string representation of the stored bytes.</returns>
    /// <remarks>
    /// This method assumes that the stored bytes are valid ASCII characters. You might want to add error handling for invalid ASCII bytes.
    /// </remarks>
    public string ToAsciiString()
    {
        return Encoding.ASCII.GetString(ToByteArray());
    }
}
