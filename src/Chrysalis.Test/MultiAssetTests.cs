using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class MultiAssetTests
{
    [Fact]
    public void TestMultiAssetFromToCbor()
    {
        // Given
        byte[] originalCborData = Convert.FromHexString("A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501");

        // When (Deserialization)
        var multiAsset = CborSerializer.Deserialize<MultiAsset>(originalCborData);

        // ... Insert assertions for individual fields here ...

        // When (Serialization)
        byte[] serializedCborData = CborSerializer.Serialize(multiAsset);

        // Then
        Assert.True(originalCborData.SequenceEqual(serializedCborData));
    }

    [Fact]
    public void TestMultiAssetFromToHex()
    {
        // Given
        string originalHex = "A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501";
        MultiAsset? originalMultiAsset = ByteConvertibleFactory.FromHex<MultiAsset>(originalHex);

        // When
        string convertedHex = originalMultiAsset?.ToHexString()!;

        // Then
        Assert.Equal(originalHex.ToLowerInvariant(), convertedHex);
    }

    [Fact]
    public void TestMultiAssetFromToBytes()
    {
        // Given
        byte[] originalBytes = Convert.FromHexString("A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501");
        MultiAsset? originalMultiAsset = ByteConvertibleFactory.FromBytes<MultiAsset>(originalBytes);

        // When
        byte[]? convertedBytes = originalMultiAsset?.ToByteArray()!;

        // Then
        Assert.True(originalBytes.SequenceEqual(convertedBytes));
    }
}
