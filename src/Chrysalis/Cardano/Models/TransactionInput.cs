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
[CborType(CborRepresentation.Tuple)]  // Assume that CborRepresentation is an enum specifying array or map representation
public class TransactionInput
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.ByteString)]
    public string? TransactionId { get; set; }

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.UInt32)]
    public uint Index { get; set; }

    public override int GetHashCode()
    {
        return (TransactionId?.GetHashCode() ?? 0) ^ Index.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TransactionInput other)
        {
            return false;
        }

        return TransactionId == other.TransactionId && Index == other.Index;
    }
}