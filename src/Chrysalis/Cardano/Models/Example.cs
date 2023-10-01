using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

[CborType(CborRepresentation.Tuple)]
public record Example
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.ByteString)]
    public string TransactionId { get; set; } = string.Empty;

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Int32)]
    public int Index { get; set; } = 0;

    [CborProperty(CborRepresentation.Int32, 2, CborRepresentation.Array)]
    public Example? Examples { get; set; }
}
