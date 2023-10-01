using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <remarks>
/// CDDL definition:
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
[CborType(CborRepresentation.Record)]
public class TransactionBody
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.Set)]
    public TransactionInputs Inputs { get; set; } = new(); // Assuming TransactionInputs is a collection

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Array)]
    public TransactionOutputs Outputs { get; set; } = new(); // Assuming TransactionOutputs is a collection

    // [CborProperty(CborRepresentation.Int32, 2, CborRepresentation.Int32)]
    // public CoinValue Fee { get; set; } = new();

    [CborProperty(CborRepresentation.Int32, 3, CborRepresentation.Int64)]
    public ulong? TTL { get; set; }
}

