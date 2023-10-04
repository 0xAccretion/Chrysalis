using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class ValueTests
{
    [Fact]
    public void TestIValueSerializationAndDeserialization()
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

        var multiAssetValue = new Value(1452470, originalMultiAsset);

        var coinValue = new Value(1452470);

        // Act - MultiAssetValue
        byte[] serializedMultiAssetValue = CborSerializerV2.Serialize(multiAssetValue);
        var deserializedMultiAssetValue = CborSerializerV2.Deserialize<Value>(serializedMultiAssetValue);

        // Assert - MultiAssetValue
        Assert.NotNull(deserializedMultiAssetValue);
        Assert.Equal(multiAssetValue.Coin, deserializedMultiAssetValue!.Coin);
        Assert.Equal(multiAssetValue.MultiAsset, deserializedMultiAssetValue.MultiAsset);  // Adjust this as needed

        // Act - CoinValue
        byte[] serializedCoinValue = CborSerializerV2.Serialize(coinValue);
        var deserializedCoinValue = CborSerializerV2.Deserialize<Value>(serializedCoinValue);

        // Assert - CoinValue
        Assert.NotNull(deserializedCoinValue);
        Assert.Equal(coinValue.Coin, deserializedCoinValue!.Coin);
    }
}
