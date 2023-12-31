using System.Formats.Cbor;
using Blake2Fast;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <summary>
/// Represents a Cardano transaction.
/// </summary>
/// <remarks>
/// CDDL definition: 
/// transaction =
///  [ transaction_body
///  , transaction_witness_set
///  , bool
///  , auxiliary_data / null
///  ]
/// </remarks>
[CborType(CborRepresentation.Tuple)]
public class Transaction
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.Record)]
    public TransactionBody TransactionBody { get; set; } = new TransactionBody();

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Ignore)]
    public object TransactionWitnessSet { get; set; } = default!;

    [CborProperty(CborRepresentation.Int32, 2, CborRepresentation.Bool)]
    public bool IsValid { get; set; }

    [CborProperty(CborRepresentation.Int32, 3, CborRepresentation.Ignore)]
    public string? AuxiliaryData { get; set; }

    public string Id => Convert.ToHexString(Blake2b.ComputeHash(32, CborSerializerV2.Serialize(TransactionBody))).ToLowerInvariant();
}