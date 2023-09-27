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
public class TransactionOutput  : ByteConvertibleCborBase, ICborObject<TransactionOutput>
{
    /// <summary>
    /// Gets or sets the address of the transaction output.
    /// </summary>
    public ByteString Address { get; set; } = new ByteString();

    /// <summary>
    /// Gets or sets the amount of the transaction output.
    /// </summary>
    public IValue Amount { get; set; } = new CoinValue();

    /// <summary>
    /// Gets or sets the datum hash of the transaction output.
    /// </summary>
    public ByteString? DatumHash { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionOutput"/> class.
    /// </summary>
    public TransactionOutput() : base(Array.Empty<byte>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionOutput"/> class from a hexadecimal string.
    /// </summary>
    /// <param name="hex">The hexadecimal string.</param>
    public TransactionOutput(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionOutput"/> class from a byte array.
    /// </summary>
    /// <param name="cborData">The byte array.</param>
    public TransactionOutput(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    /// <summary>
    /// Deserializes the transaction output from a byte array.
    /// </summary>
    /// <param name="data">The byte array.</param>
    /// <returns>The deserialized transaction output.</returns>
    public override TransactionOutput FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        return FromCbor(reader);
    }

    /// <summary>
    /// Deserializes the transaction output from a CBOR reader.
    /// </summary>
    /// <param name="reader">The CBOR reader.</param>
    /// <returns>The deserialized transaction output.</returns>
    public override TransactionOutput FromCbor(CborReader reader)
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

    /// <summary>
    /// Serializes the transaction output to a byte array.
    /// </summary>
    /// <returns>The serialized transaction output.</returns>
    public override byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        return ToCbor(writer).Encode();
    }

    public override CborWriter ToCbor(CborWriter writer)
    {
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
        return writer;
    }

    /// <inheritdoc/>
    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    /// <inheritdoc/>
    public override string ToHexString()
    {
        return Convert.ToHexString(ToCbor()).ToLowerInvariant();
    }
}
