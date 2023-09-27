using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <remarks>
/// CDDL definition:
/// FILEPATH: /Users/rawriclark/Projects/Chrysalis/src/Chrysalis/Cardano/Models/TransactionBody.cs
/// transaction_body =
///  { 0 : set<transaction_input>    ; inputs
///  , 1 : [* transaction_output]
///  , 2 : coin                      ; fee
///  , ? 3 : uint                    ; time to live
///  , ? 4 : [* certificate]
///  , ? 5 : withdrawals
///  , ? 6 : update
///  , ? 7 : auxiliary_data_hash
///  , ? 8 : uint                    ; validity interval start
///  , ? 9 : mint
///  , ? 11 : script_data_hash       ; New
///  , ? 13 : set<transaction_input> ; Collateral ; new
///  , ? 14 : required_signers       ; New
///  , ? 15 : network_id             ; New
///  }
/// </remarks>
public class TransactionBody : ByteConvertibleBase, ICborObject<TransactionBody>
{
    public TransactionInputs Inputs { get; set; } = new();
    public TransactionOutputs Outputs { get; set; } = new();
    public CoinValue Fee { get; set; } = new();
    public ulong? TTL { get; set; }

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
                case 3:
                    TTL = reader.ReadUInt64();
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
        int mapLength = 3;
        if (TTL.HasValue)
        {
            mapLength++;
        }

        writer.WriteStartMap(mapLength);

        writer.WriteInt32(0);
        Inputs.ToCbor(writer);

        writer.WriteInt32(1);
        Outputs.ToCbor(writer);

        writer.WriteInt32(2);
        writer.WriteUInt64(Fee.Coin);

        if (TTL.HasValue)
        {
            writer.WriteInt32(3);
            writer.WriteUInt64(TTL.Value);
        }

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

