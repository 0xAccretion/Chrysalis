using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <remarks>
/// CDDL definition:
/// transaction_body =
/// { 0 : set<transaction_input>    ; inputs
/// , 1 : [* transaction_output]
/// , 2 : coin                      ; fee
/// }
/// </remarks>
public class TransactionBody : ByteConvertibleBase, ICborObject<TransactionBody>
{
    public TransactionInputs Inputs { get; set; } = new();
    public TransactionOutputs Outputs { get; set; } = new();
    public CoinValue Fee { get; set; } = new();

    public TransactionBody() : base(Array.Empty<byte>()) { }

    public TransactionBody(string hex) : base(Convert.FromHexString(hex))
    {
        FromCbor(Convert.FromHexString(hex));
    }

    public TransactionBody(byte[] cborData) : base(cborData)
    {
        FromCbor(cborData);
    }

    public TransactionBody FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        return FromCbor(reader);
    }

    public TransactionBody FromCbor(CborReader reader)
    {
        reader.ReadStartMap();
        while (reader.PeekState() != CborReaderState.EndMap)
        {
            var key = reader.ReadInt32();
            switch (key)
            {
                case 0:
                    Inputs.FromCbor(reader);
                    break;
                case 1:
                    Outputs.FromCbor(reader);
                    break;
                case 2:
                    Fee = new CoinValue().FromCbor(reader);
                    break;
                default:
                    reader.SkipValue();
                    break;
            }
        }
        reader.ReadEndMap();
        return this;
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        return ToCbor(writer).Encode();
    }

    public CborWriter ToCbor(CborWriter writer)
    {
        writer.WriteStartMap(3);

        writer.WriteInt32(0);
        Inputs.ToCbor(writer);

        writer.WriteInt32(1);
        Outputs.ToCbor(writer);

        writer.WriteInt32(2);
        writer.WriteUInt64(Fee.Coin);

        writer.WriteEndMap();
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

