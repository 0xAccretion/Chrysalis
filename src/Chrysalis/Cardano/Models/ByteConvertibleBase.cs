namespace Chrysalis.Cardano.Models;

/// <summary>
/// Base class for objects that can be converted to and from byte arrays and hexadecimal strings.
/// </summary>
public abstract class ByteConvertibleBase : IByteConvertible, IEquatable<ByteConvertibleBase>
{
    protected byte[] _rawBytes;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteConvertibleBase"/> class with the specified byte array.
    /// </summary>
    /// <param name="bytes">The byte array to initialize the object with.</param>
    protected ByteConvertibleBase(byte[] bytes)
    {
        _rawBytes = bytes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteConvertibleBase"/> class with the specified hexadecimal string.
    /// </summary>
    /// <param name="hexString">The hexadecimal string to initialize the object with.</param>
    protected ByteConvertibleBase(string hexString)
    {
        _rawBytes = Convert.FromHexString(hexString);
    }

    /// <summary>
    /// Returns the hexadecimal string representation of the object.
    /// </summary>
    /// <returns>The hexadecimal string representation of the object.</returns>
    public override string ToString()
    {
        return ToHexString();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public bool Equals(ByteConvertibleBase? other)
    {
        if (other == null) return false;
        return Enumerable.SequenceEqual(_rawBytes, other._rawBytes);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ByteConvertibleBase);
    }

    /// <summary>
    /// Returns a hash code for the current object.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(_rawBytes);
        return BitConverter.ToInt32(hashBytes.Take(4).ToArray());
    }
    
    /// <summary>
    /// Converts the object to a byte array.
    /// </summary>
    /// <returns>A byte array representation of the object.</returns>
    public virtual byte[] ToByteArray()
    {
        return _rawBytes;
    }

    /// <summary>
    /// Converts the raw bytes of the object to a lowercase hexadecimal string.
    /// </summary>
    /// <returns>A lowercase hexadecimal string representation of the object's raw bytes.</returns>
    public virtual string ToHexString()
    {
        return Convert.ToHexString(_rawBytes).ToLowerInvariant();
    }
}
