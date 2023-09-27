using Chrysalis.Cardano.Models;

namespace Chrysalis.Test;

public class AssetNameTests
{
    [Fact]
    public void TestToAsciiString()
    {
        // Arrange
        string originalHex = "48595045534b554c4c535f56545f505f45";
        string expectedAscii = "HYPESKULLS_VT_P_E";

        // Act
        AssetName assetName = new(originalHex);
        string actualAscii = assetName.ToAsciiString();

        // Assert
        Assert.Equal(expectedAscii, actualAscii);
    }
}
