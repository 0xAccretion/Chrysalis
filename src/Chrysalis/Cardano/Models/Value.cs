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

public class CoinValue : ByteConvertibleBase, ICborObject, IValue
{
    public ulong Coin { get; set; }

    public CoinValue() : base(Array.Empty<byte>())
    {
        Coin = 0;
    }

    public CoinValue(ulong coin) : base(Array.Empty<byte>())
    {
        Coin = coin;
    }

    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteUInt64(Coin);
        return writer.Encode();
    }

    public void FromCbor(CborReader reader)
    {
        Coin = reader.ReadUInt64();
    }
}

public class MultiAssetValue : ByteConvertibleBase, ICborObject, IValue
{
    public ulong Coin { get; set; }
    public MultiAsset Assets { get; set; }

    public MultiAssetValue() : base(Array.Empty<byte>())
    {
        Coin = 0;
        Assets = new MultiAsset(); // Initialize with a default constructor or however you prefer
    }

    public MultiAssetValue(ulong coin, MultiAsset assets) : base(Array.Empty<byte>())
    {
        Coin = coin;
        Assets = assets;
    }

    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
    }

    public void FromCbor(CborReader reader)
    {
        reader.ReadStartArray();
        Coin = reader.ReadUInt64();
        Assets = new MultiAsset();
        Assets.FromCbor(reader);
        reader.ReadEndArray();
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteStartArray(2);
        writer.WriteUInt64(Coin);
        writer.WriteEncodedValue(Assets.ToCbor());
        writer.WriteEndArray();
        return writer.Encode();
    }
}