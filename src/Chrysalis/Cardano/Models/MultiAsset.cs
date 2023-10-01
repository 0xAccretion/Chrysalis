using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

/// <remarks>
/// CDDL definition:
/// multiasset<a> = { * policy_id => { * asset_name => a } }
/// </remarks>
[CborType(CborRepresentation.Map)]
public class MultiAsset : Dictionary<string, Asset>
{
    // No additional properties, it's just a specialized Dictionary.
}