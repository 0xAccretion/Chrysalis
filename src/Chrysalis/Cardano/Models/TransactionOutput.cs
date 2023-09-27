using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <summary>
/// Represents an output of a Cardano transaction, consisting of an address, an amount, and an optional datum hash.
/// </summary>
/// <remarks>
/// CDDL definition:
/// transaction_output =
///   [ address
///   , amount : value
///   , ? datum_hash : hash32 ; New
///   ]
/// </remarks>
public class TransactionOutput : ByteConvertibleBase, ICborObject<TransactionOutput>
{
    public ByteString Address { get; set; } = new ByteString();
    public IValue Amount { get; set; } = new CoinValue();
    public ByteString? DatumHash { get; set; }

    public TransactionOutput() : base(Array.Empty<byte>())
    {
    }

    public TransactionOutput(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    public TransactionOutput(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    public TransactionOutput FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        return FromCbor(reader);
    }

    public TransactionOutput FromCbor(CborReader reader)
    {
        reader.ReadStartArray();
        Address = new ByteString(reader.ReadByteString());

        Amount = reader.PeekState() switch
        {
            CborReaderState.UnsignedInteger => new CoinValue(reader.ReadUInt64()),
            CborReaderState.StartArray => new MultiAssetValue().FromCbor(reader),
            _ => throw new CborContentException($"Unexpected CBOR reader state: {reader.PeekState()}")
        };

        if (reader.PeekState() != CborReaderState.EndArray)
        {
            DatumHash = new ByteString(reader.ReadByteString());
        }
        reader.ReadEndArray();
        return this;
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteStartArray(DatumHash == null ? 2 : 3);
        writer.WriteByteString(Address.ToByteArray());

        if (Amount is MultiAssetValue multiAssetValue)
        {
            writer.WriteEncodedValue(multiAssetValue.ToCbor());
        }
        else if (Amount is CoinValue coinValue)
        {
            writer.WriteEncodedValue(coinValue.ToCbor());
        }
        else
        {
            throw new CborContentException($"Unexpected amount type: {Amount.GetType()}");
        }

        if (DatumHash != null)
        {
            writer.WriteByteString(DatumHash.ToByteArray());
        }

        writer.WriteEndArray();
        return writer.Encode();
    }

    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    public override string ToHexString()
    {
        return Convert.ToHexString(ToCbor()).ToLowerInvariant();
    }
}
