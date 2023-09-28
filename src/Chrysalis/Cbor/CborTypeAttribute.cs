namespace Chrysalis.Cbor;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class CborTypeAttribute(CborRepresentation representation) : Attribute
{
    public CborRepresentation CborRepresentation { get; } = representation;
}