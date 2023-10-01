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
[CborType(CborRepresentation.Tuple)] // Assume that CborRepresentation is an enum specifying array or map representation
public class TransactionOutput
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.ByteString)]
    public string Address { get; set; } = string.Empty;

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Tuple)] 
    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Int64, true)] 
    public IValue Amount { get; set; } = new CoinValue();

    [CborProperty(CborRepresentation.Int32, 2, CborRepresentation.ByteString)]
    public string? DatumHash { get; set; }
}