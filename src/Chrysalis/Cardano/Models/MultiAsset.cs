using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;


/// <summary>
/// Represents a multi-asset object in the Cardano blockchain.
/// </summary>
/// <remarks>
/// A multi-asset object is a collection of assets that can be stored on the Cardano blockchain. It is represented as a dictionary of policy IDs, where each policy ID maps to a dictionary of asset names and their corresponding values.
/// 
/// CDDL: multiasset<a> = { * policy_id => { * asset_name => a } }
/// policy_id = scripthash
/// asset_name = bytes .size (0..32)
/// </remarks>
public class MultiAsset : ByteConvertibleBase, ICborObject
{
    private readonly Dictionary<ByteString, Dictionary<AssetName, ulong>> _assets = [];

    public MultiAsset() : base(Array.Empty<byte>()) { }

    public MultiAsset(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    public MultiAsset(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    public void FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
    }

    public void FromCbor(CborReader reader)
    {
        reader.ReadStartMap();

        while (reader.PeekState() != CborReaderState.EndMap)
        {
            var policyId = ByteConvertibleFactory.FromBytes<ByteString>(reader.ReadByteString())!;
            var assetMap = new Dictionary<AssetName, ulong>();

            reader.ReadStartMap();
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var assetName = ByteConvertibleFactory.FromBytes<AssetName>(reader.ReadByteString())!;
                var value = reader.ReadUInt64();
                assetMap[assetName] = value;
            }

            reader.ReadEndMap();
            _assets[policyId] = assetMap;
        }

        reader.ReadEndMap();
    }

    public byte[] ToCbor()
    {
        var writer = new CborWriter(CborConformanceMode.Strict);
        writer.WriteStartMap(_assets.Count);

        foreach (var policyEntry in _assets)
        {
            writer.WriteByteString(policyEntry.Key.ToByteArray());

            writer.WriteStartMap(policyEntry.Value.Count);
            foreach (var assetEntry in policyEntry.Value)
            {
                writer.WriteByteString(assetEntry.Key.ToByteArray());
                writer.WriteUInt64(assetEntry.Value);
            }

            writer.WriteEndMap();
        }

        writer.WriteEndMap();
        return writer.Encode();
    }

    public override byte[] ToByteArray()
    {
        return ToCbor(); // Assuming ToCbor is the byte-equivalent for this class
    }

    public override string ToHexString()
    {
        return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
    }
}
