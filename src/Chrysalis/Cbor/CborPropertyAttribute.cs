namespace Chrysalis.Cbor;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class CborPropertyAttribute(CborRepresentation indexType, object indexValue, CborRepresentation valueType, bool isBasicType = false) : Attribute
{
    public CborRepresentation IndexType { get; } = indexType;
    public object IndexValue { get; } = indexValue;
    public CborRepresentation ValueType { get; } = valueType;
    public bool IsBasicType { get; set; } = isBasicType;
}