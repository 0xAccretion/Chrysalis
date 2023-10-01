using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models
{
    /// <summary>
    /// Represents a collection of transaction inputs.
    /// </summary>
    [CborType(CborRepresentation.Set)]
    public class TransactionInputs : HashSet<TransactionInput>
    {
    }
}