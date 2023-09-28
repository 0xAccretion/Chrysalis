namespace Chrysalis.Cbor;

public class CborIndex(CborRepresentation type, object value)
{
    public CborRepresentation Type { get; } = type;
    public object Value { get; } = value;
}
