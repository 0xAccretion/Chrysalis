using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/**
 * <summary>
 * Represents an input to a Cardano transaction, consisting of a transaction ID and an index.
 * </summary>
 * <remarks>
 * CDDL definition:
 * transaction_input = [ transaction_id : $hash32
 *                     , index : uint
 *                     ]
 * </remarks>
 */
public class TransactionInput : ByteConvertibleBase, ICborObject
{
    public ByteString? TransactionId { get; set; }
    public int Index { get; set; }

    public TransactionInput() : base(Array.Empty<byte>()) // or another suitable default value
    {
        // Initialize TransactionId and Index if necessary
        TransactionId = null;
        Index = 0;
    }

    public TransactionInput(string hex) : base(Array.Empty<byte>())
    {
        // Initialize TransactionId and Index based on hex or leave them
        // Maybe call FromCbor(Convert.FromHexString(hex));
        FromCbor(Convert.FromHexString(hex));
    }

    public TransactionInput(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        reader.ReadStartArray();

        // Create a new TransactionId object from the byte array
        TransactionId = ByteConvertibleFactory.FromBytes<ByteString>(reader.ReadByteString());

        Index = reader.ReadInt32();
        reader.ReadEndArray();
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteStartArray(2);

        // Write transaction ID as a byte string
        writer.WriteByteString(TransactionId?.ToByteArray() ?? []);

        // Write index as a uint
        writer.WriteInt32(Index);

        writer.WriteEndArray();
        return writer.Encode();
    }

    public override byte[] ToByteArray()
    {
        return ToCbor(); // Assuming ToCbor is the byte-equivalent for this class
    }

    public override string ToHexString()
    {
        return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
    }
}