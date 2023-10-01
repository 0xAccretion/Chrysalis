using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class AssetTests
{
    [Fact]
    public void TestAssetFromToCbor()
    {
        // Arrange
        var originalAsset = new Asset
        {
            { "48595045534b554c4c535f56545f505f45", 1 },
            { "48595045534b554c4c535f56545f565f43", 2 },
            { "48595045534b554c4c535f56545f4d5f4545", 3 },
            { "48595045534b554c4c535f56545f41435f4545", 4 }
        };

        // Act
        // Serialization
        byte[] serializedCborData = CborSerializerV2.Serialize(originalAsset);

        // Deserialization
        var deserializedAsset = CborSerializerV2.Deserialize<Asset>(serializedCborData);

        // Assert
        Assert.NotNull(deserializedAsset);
        Assert.Equal(4, deserializedAsset.Count);
        Assert.True(deserializedAsset.ContainsKey("48595045534b554c4c535f56545f505f45"));
        Assert.True(deserializedAsset.ContainsKey("48595045534b554c4c535f56545f565f43"));
        Assert.True(deserializedAsset.ContainsKey("48595045534b554c4c535f56545f4d5f4545"));
        Assert.True(deserializedAsset.ContainsKey("48595045534b554c4c535f56545f41435f4545"));
        Assert.Equal(1u, deserializedAsset["48595045534b554c4c535f56545f505f45"]);
        Assert.Equal(2u, deserializedAsset["48595045534b554c4c535f56545f565f43"]);
        Assert.Equal(3u, deserializedAsset["48595045534b554c4c535f56545f4d5f4545"]);
        Assert.Equal(4u, deserializedAsset["48595045534b554c4c535f56545f41435f4545"]);
    }
}