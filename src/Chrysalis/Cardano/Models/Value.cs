using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <summary>
/// Represents a value in Cardano, which can be either a single coin or a combination of coins and other assets.
/// CDDL: value = coin / [coin,multiasset<uint>]
/// </summary>
public interface IValue
{
    /// <summary>
    /// The amount of the base currency (coin) in the value.
    /// </summary>
    ulong Coin { get; set; }
}

/// <summary>
/// Represents a value in Cardano that consists of a single coin.
/// </summary>
public class CoinValue : ByteConvertibleBase, ICborObject, IValue
{
    /// <summary>
    /// The amount of the base currency (coin) in the value.
    /// </summary>
    public ulong Coin { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinValue"/> class with a coin value of 0.
    /// </summary>
    public CoinValue() : base(Array.Empty<byte>())
    {
        Coin = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinValue"/> class with the specified coin value.
    /// </summary>
    /// <param name="coin">The amount of the base currency (coin) in the value.</param>
    public CoinValue(ulong coin) : base(Array.Empty<byte>())
    {
        Coin = coin;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinValue"/> class with the specified hex string.
    /// </summary>
    /// <param name="hex">The hex string to initialize the instance with.</param>
    public CoinValue(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinValue"/> class with the specified CBOR data.
    /// </summary>
    /// <param name="cborData">The CBOR data to initialize the instance with.</param>
    public CoinValue(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    /// <summary>
    /// Deserializes the instance from the specified CBOR data.
    /// </summary>
    /// <param name="data">The CBOR data to deserialize the instance from.</param>
    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
    }

    /// <summary>
    /// Serializes the instance to CBOR data.
    /// </summary>
    /// <returns>The serialized CBOR data.</returns>
    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteUInt64(Coin);
        return writer.Encode();
    }

    /// <summary>
    /// Deserializes the instance from the specified CBOR reader.
    /// </summary>
    /// <param name="reader">The CBOR reader to deserialize the instance from.</param>
    public void FromCbor(CborReader reader)
    {
        Coin = reader.ReadUInt64();
    }
    
    /// <summary>
    /// Serializes the instance to a byte array.
    /// </summary>
    /// <returns>The serialized byte array.</returns>
    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    /// <summary>
    /// Serializes the instance to a hex string.
    /// </summary>
    /// <returns>The serialized hex string.</returns>
    public override string ToHexString()
    {
        return Convert.ToHexString(ToCbor()).ToLowerInvariant();
    }
}

/// <summary>
/// Represents a value in Cardano that consists of a combination of coins and other assets.
/// </summary>
public class MultiAssetValue : ByteConvertibleBase, ICborObject, IValue
{
    /// <summary>
    /// The amount of the base currency (coin) in the value.
    /// </summary>
    public ulong Coin { get; set; } = 0;

    /// <summary>
    /// The assets included in the value.
    /// </summary>
    public MultiAsset Assets { get; set; } = new MultiAsset();

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAssetValue"/> class with a coin value of 0 and no assets.
    /// </summary>
    public MultiAssetValue() : base(Array.Empty<byte>())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAssetValue"/> class with the specified coin value and assets.
    /// </summary>
    /// <param name="coin">The amount of the base currency (coin) in the value.</param>
    /// <param name="assets">The assets included in the value.</param>
    public MultiAssetValue(ulong coin, MultiAsset assets) : base(Array.Empty<byte>())
    {
        Coin = coin;
        Assets = assets;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAssetValue"/> class with the specified hex string.
    /// </summary>
    /// <param name="hex">The hex string to initialize the instance with.</param>
    public MultiAssetValue(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAssetValue"/> class with the specified CBOR data.
    /// </summary>
    /// <param name="cborData">The CBOR data to initialize the instance with.</param>
    public MultiAssetValue(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    /// <summary>
    /// Deserializes the instance from the specified CBOR data.
    /// </summary>
    /// <param name="data">The CBOR data to deserialize the instance from.</param>
    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
    }

    /// <summary>
    /// Deserializes the instance from the specified CBOR reader.
    /// </summary>
    /// <param name="reader">The CBOR reader to deserialize the instance from.</param>
    public void FromCbor(CborReader reader)
    {
        reader.ReadStartArray();
        Coin = reader.ReadUInt64();
        Assets = new MultiAsset();
        Assets.FromCbor(reader);
        reader.ReadEndArray();
    }

    /// <summary>
    /// Serializes the instance to CBOR data.
    /// </summary>
    /// <returns>The serialized CBOR data.</returns>
    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteStartArray(2);
        writer.WriteUInt64(Coin);
        writer.WriteEncodedValue(Assets.ToCbor());
        writer.WriteEndArray();
        return writer.Encode();
    }

    /// <summary>
    /// Serializes the instance to a byte array.
    /// </summary>
    /// <returns>The serialized byte array.</returns>
    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    /// <summary>
    /// Serializes the instance to a hex string.
    /// </summary>
    /// <returns>The serialized hex string.</returns>
    public override string ToHexString()
    {
        return Convert.ToHexString(ToCbor()).ToLowerInvariant();
    }
}