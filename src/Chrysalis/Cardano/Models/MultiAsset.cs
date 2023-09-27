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
public class MultiAsset : ByteConvertibleBase, ICborObject<MultiAsset>
{
    private readonly Dictionary<ByteString, Dictionary<AssetName, ulong>> _assets = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAsset"/> class.
    /// </summary>
    public MultiAsset() : base(Array.Empty<byte>()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAsset"/> class from a hexadecimal string.
    /// </summary>
    /// <param name="hex">The hexadecimal string.</param>
    public MultiAsset(string hex) : base(Array.Empty<byte>())
    {
        FromCbor(Convert.FromHexString(hex));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiAsset"/> class from a byte array.
    /// </summary>
    /// <param name="cborData">The byte array.</param>
    public MultiAsset(byte[] cborData) : base(Array.Empty<byte>())
    {
        FromCbor(cborData);
    }

    /// <summary>
    /// Deserializes a <see cref="MultiAsset"/> object from a byte array.
    /// </summary>
    /// <param name="data">The byte array.</param>
    /// <returns>The deserialized <see cref="MultiAsset"/> object.</returns>
    public MultiAsset FromCbor(byte[] data)
    {
        var reader = new CborReader(data);
        FromCbor(reader);
        return this;
    }

    /// <summary>
    /// Deserializes a <see cref="MultiAsset"/> object from a <see cref="CborReader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="CborReader"/>.</param>
    /// <returns>The deserialized <see cref="MultiAsset"/> object.</returns>
    public MultiAsset FromCbor(CborReader reader)
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
        return this;
    }

    /// <summary>
    /// Serializes the <see cref="MultiAsset"/> object to a byte array.
    /// </summary>
    /// <returns>The serialized byte array.</returns>
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

    /// <summary>
    /// Serializes the <see cref="MultiAsset"/> object to a byte array.
    /// </summary>
    /// <returns>The serialized byte array.</returns>
    public override byte[] ToByteArray()
    {
        return ToCbor();
    }

    /// <summary>
    /// Serializes the <see cref="MultiAsset"/> object to a hexadecimal string.
    /// </summary>
    /// <returns>The serialized hexadecimal string.</returns>
    public override string ToHexString()
    {
        return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
    }
}
