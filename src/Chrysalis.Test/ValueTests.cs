using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class ValueTests
{
    [Fact]
    public void TestCoinValueFromToCbor()
    {
        // Given
        byte[] originalCborData = Convert.FromHexString("1a001629b6");
        var expectedCoin = 1_452_470UL; // Replace with expected value

        // When (Deserialization)
        var coinValue = CborSerializer.Deserialize<CoinValue>(originalCborData);

        // Then
        Assert.Equal(expectedCoin, coinValue.Coin);

        // When (Serialization)
        byte[] serializedCborData = CborSerializer.Serialize(coinValue);

        // Then
        Assert.True(originalCborData.SequenceEqual(serializedCborData));
    }

    [Fact]
    public void TestMultiAssetValueFromToCbor()
    {
        // Given
        byte[] originalCborData = Convert.FromHexString("821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501");

        // Create expected values
        var expectedCoin = 1_452_470UL; // Replace with expected value

        // When (Deserialization)
        var multiAssetValue = CborSerializer.Deserialize<MultiAssetValue>(originalCborData);

        // Then
        Assert.Equal(expectedCoin, multiAssetValue.Coin);
        // Add asserts to compare expectedAssets and multiAssetValue.Assets

        // When (Serialization)
        byte[] serializedCborData = CborSerializer.Serialize(multiAssetValue);

        // Then
        Assert.True(originalCborData.SequenceEqual(serializedCborData));
    }
}
