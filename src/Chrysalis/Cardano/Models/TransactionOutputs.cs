using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

public class TransactionOutputs : ByteConvertibleBase, ICborObject<TransactionOutputs>
{
    private readonly List<TransactionOutput> _outputs = [];

    public TransactionOutputs() : base(Array.Empty<byte>()) { }

    public TransactionOutputs(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    public TransactionOutputs(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    public TransactionOutputs FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
        return this;
    }

    public TransactionOutputs FromCbor(CborReader reader)
    {
        reader.ReadStartArray();
        _outputs.Clear();

        while (reader.PeekState() != CborReaderState.EndArray)
        {
            var output = new TransactionOutput(); // Assuming TransactionOutput has a method to read from CBOR
            output.FromCbor(reader);
            _outputs.Add(output);
        }

        reader.ReadEndArray();
        return this;
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        return ToCbor(writer).Encode();
    }

    public CborWriter ToCbor(CborWriter writer)
    {
        writer.WriteStartArray(_outputs.Count);

        foreach (var output in _outputs)
        {
            output.ToCbor(writer); // Assuming TransactionOutput has a method to write to CBOR
        }

        writer.WriteEndArray();
        return writer;
    }

    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    public override string ToHexString()
    {
        return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
    }
}