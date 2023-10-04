using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models
{
    /// <summary>
    /// Represents a collection of transaction outputs in a Cardano transaction.
    /// </summary>
    [CborType(CborRepresentation.Array)]
    public class TransactionOutputs : List<TransactionOutput>
    {
        
    }
}