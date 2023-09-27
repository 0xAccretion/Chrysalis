using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <summary>
/// Represents an input to a Cardano transaction, consisting of a transaction ID and an index.
/// </summary>
/// <remarks>
/// CDDL definition:
/// transaction_input = [ transaction_id : $hash32
///                     , index : uint
///                     ]
/// </remarks>
public class TransactionInput : ByteConvertibleBase, ICborObject<TransactionInput>
{
    /// <summary>
    /// Gets or sets the transaction ID.
    /// </summary>
    public ByteString? TransactionId { get; set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionInput"/> class.
    /// </summary>
    public TransactionInput() : base(Array.Empty<byte>()) // or another suitable default value
    {
        // Initialize TransactionId and Index if necessary
        TransactionId = null;
        Index = 0;
    }

    public TransactionInput(ByteString transactionId, int index) : base(Array.Empty<byte>())
    {
        TransactionId = transactionId;
        Index = index;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionInput"/> class from a hexadecimal string.
    /// </summary>
    /// <param name="hex">The hexadecimal string.</param>
    public TransactionInput(string hex) : base(Array.Empty<byte>())
    {
        // Initialize TransactionId and Index based on hex or leave them
        // Maybe call FromCbor(Convert.FromHexString(hex));
        FromCbor(Convert.FromHexString(hex));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionInput"/> class from a CBOR byte array.
    /// </summary>
    /// <param name="cborData">The CBOR byte array.</param>
    public TransactionInput(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    /// <summary>
    /// Reads the CBOR byte array and initializes the <see cref="TransactionInput"/> instance.
    /// </summary>
    /// <param name="data">The CBOR byte array.</param>
    public TransactionInput FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        return FromCbor(reader);
    }

    /// <summary>
    /// Reads the CBOR data and initializes the <see cref="TransactionInput"/> instance.
    /// </summary>
    /// <param name="reader">The CBOR reader.</param>
    public TransactionInput FromCbor(CborReader reader)
    {
        reader.ReadStartArray();

        // Create a new TransactionId object from the byte array
        TransactionId = ByteConvertibleFactory.FromBytes<ByteString>(reader.ReadByteString());

        Index = reader.ReadInt32();
        reader.ReadEndArray();
        return this;
    }

    /// <summary>
    /// Converts the <see cref="TransactionInput"/> instance to a CBOR byte array.
    /// </summary>
    /// <returns>The CBOR byte array.</returns>
    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict); 
        return ToCbor(writer).Encode();
    }

    public CborWriter ToCbor(CborWriter writer)
    {
        writer.WriteStartArray(2);

        // Write transaction ID as a byte string
        writer.WriteByteString(TransactionId?.ToByteArray() ?? []);

        // Write index as a uint
        writer.WriteInt32(Index);

        writer.WriteEndArray();
        return writer;
    }

    /// <summary>
    /// Converts the <see cref="TransactionInput"/> instance to a byte array.
    /// </summary>
    /// <returns>The byte array.</returns>
    public override byte[] ToByteArray()
    {
        return ToCbor(); // Assuming ToCbor is the byte-equivalent for this class
    }

    /// <summary>
    /// Converts the <see cref="TransactionInput"/> instance to a hexadecimal string.
    /// </summary>
    /// <returns>The hexadecimal string.</returns>
    public override string ToHexString()
    {
        return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
    }
}
