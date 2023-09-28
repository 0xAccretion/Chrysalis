namespace Chrysalis.Cbor;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class CborPropertyAttribute(CborRepresentation indexType, object indexValue, CborRepresentation valueType) : Attribute
{
    public CborRepresentation IndexType { get; } = indexType;
    public object IndexValue { get; } = indexValue;
    public CborRepresentation ValueType { get; } = valueType;
}