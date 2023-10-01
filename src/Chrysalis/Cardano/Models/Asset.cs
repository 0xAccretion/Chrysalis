using Chrysalis.Cbor;

namespace Chrysalis;

/// <remarks>
/// CDDL definition:
/// Asset = { name => coin }
/// </remarks>
[CborType(CborRepresentation.Map)]
public class Asset : Dictionary<string, ulong>
{
    // No additional properties, it's just a specialized Dictionary.
}