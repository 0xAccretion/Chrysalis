namespace Chrysalis.Cardano.Models;

/// <summary>
/// Represents a script hash in Cardano blockchain.
/// </summary>
public class ByteString : ByteConvertibleBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptHash"/> class with an empty byte array.
    /// </summary>
    public ByteString() : base(Array.Empty<byte>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptHash"/> class with the specified byte array.
    /// </summary>
    /// <param name="bytes">The byte array to initialize the script hash with.</param>
    public ByteString(byte[] bytes) : base(bytes)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptHash"/> class with the specified hexadecimal string.
    /// </summary>
    /// <param name="hex">The hexadecimal string to initialize the script hash with.</param>
    public ByteString(string hex) : base(hex)
    {
    }
}

