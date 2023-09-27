using System.Text;

namespace Chrysalis.Cardano.Models;

public class AssetName : ByteString
{
    // Default constructor, uses base class constructor
    public AssetName() : base()
    {
    }

    // Constructor accepting byte array, uses base class constructor
    public AssetName(byte[] bytes) : base(bytes)
    {
    }

    // Constructor accepting hex string, uses base class constructor
    public AssetName(string hex) : base(hex)
    {
    }

    // Method to convert the hex or bytes to ASCII
    public string ToAsciiString()
    {
        // Note: Assumes that the stored bytes are valid ASCII characters.
        // You might want to add error handling for invalid ASCII bytes.
        return Encoding.ASCII.GetString(ToByteArray());
    }
}