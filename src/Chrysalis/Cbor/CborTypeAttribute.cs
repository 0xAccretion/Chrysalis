namespace Chrysalis.Cbor;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
sealed class CborTypeAttribute(CborRepresentation representation) : Attribute
{
    public CborRepresentation Representation { get; } = representation;
    public bool IsBasicType { get; set; }

    public CborTypeAttribute(CborRepresentation representation, bool isBasicType) : this(representation)
    {
        IsBasicType = isBasicType;
    }
}
