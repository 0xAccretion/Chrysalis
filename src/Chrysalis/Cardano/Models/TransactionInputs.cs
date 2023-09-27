using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

public class TransactionInputs : ByteConvertibleCborBase, ICborObject<TransactionInputs>
{
    private readonly HashSet<TransactionInput> _transactionInputs = [];

    public TransactionInputs() : base(Array.Empty<byte>()) { }

    public TransactionInputs(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    public TransactionInputs(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    public override TransactionInputs FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
        return this;
    }

    public override TransactionInputs FromCbor(CborReader reader)
    {
        reader.ReadStartArray();

        while (reader.PeekState() != CborReaderState.EndArray)
        {
            var transactionInput =  new TransactionInput().FromCbor(reader);
            if (transactionInput != null)
            {
                _transactionInputs.Add(transactionInput);
            }
        }

        reader.ReadEndArray();
        return this;
    }

    public override byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        return ToCbor(writer).Encode();
    }

    public override CborWriter ToCbor(CborWriter writer)
    {
        writer.WriteStartArray(_transactionInputs.Count);

        foreach (var input in _transactionInputs)
        {
            input.ToCbor(writer);
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