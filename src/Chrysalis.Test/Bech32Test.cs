namespace Chrysalis.Test;

public class Bech32Test
{
    [Fact]
    public void TestBech32Encoding()
    {
        // Arrange
        var hrp = "addr"; // For Shelley addresses
        byte[] data = Convert.FromHexString("019493315cd92eb5d8c4304e67b7e16ae36d61d34502694657811a2c8e337b62cfff6403a06a3acbc34f8c46003c69fe79a3628cefa9c47251");

        // You can find a real-world example or use a known good library to generate an expected result.
        var expectedBech32String = "addr1qx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer3n0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgse35a3x"; // replace ... with the expected encoding

        // Act
        var result = Bech32.Encode(hrp, data);
        (var decodedHrp, var payload) = Bech32.Decode(result);

        // Assert
        Assert.Equal(expectedBech32String, result);
        Assert.Equal(hrp, decodedHrp);
        Assert.True(data.SequenceEqual(payload));
    }
}
