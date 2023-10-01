using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class MultiAssetTests
{
    [Fact]
    public void TestMultiAssetFromToCbor()
    {
        // Arrange

        var originalAsset = new Asset
        {
            { "48595045534b554c4c535f56545f505f45", 1 },
            { "48595045534b554c4c535f56545f565f43", 2 },
            { "48595045534b554c4c535f56545f4d5f4545", 3 },
            { "48595045534b554c4c535f56545f41435f4545", 4 }
        };


        var originalMultiAsset = new MultiAsset
        {
            { "6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56", originalAsset }
        };

        // Act
        // Serialization
        byte[] serializedCborData = CborSerializerV2.Serialize(originalMultiAsset);

        // Deserialization
        var deserializedMultiAsset = CborSerializerV2.Deserialize<MultiAsset>(serializedCborData);

        // Assert
        Assert.NotNull(deserializedMultiAsset);
        Assert.Single(deserializedMultiAsset);
        Assert.Equal(4, deserializedMultiAsset["6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"].Count);

        Assert.True(deserializedMultiAsset.ContainsKey("6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"));
        Assert.Equal(1u, deserializedMultiAsset["6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"]["48595045534b554c4c535f56545f505f45"]);

        Assert.True(deserializedMultiAsset.ContainsKey("6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"));
        Assert.Equal(2u, deserializedMultiAsset["6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"]["48595045534b554c4c535f56545f565f43"]);

        Assert.True(deserializedMultiAsset.ContainsKey("6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"));
        Assert.Equal(3u, deserializedMultiAsset["6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"]["48595045534b554c4c535f56545f4d5f4545"]);

        Assert.True(deserializedMultiAsset.ContainsKey("6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"));
        Assert.Equal(4u, deserializedMultiAsset["6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56"]["48595045534b554c4c535f56545f41435f4545"]);
    }
}