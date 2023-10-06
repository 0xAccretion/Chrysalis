namespace Chrysalis.Test;

public class Bech32Test
{
    [Fact]
    public void TestBech32Encoding()
    {
        // Arrange
        var hrp = "addr"; // For Shelley addresses
        byte[] data = Convert.FromHexString("01e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a37003ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a7");

        // You can find a real-world example or use a known good library to generate an expected result.
        var expectedBech32String = "addr1q8nrqg4s73skqfyyj69mzr7clpe8s7ux9t8z6l55x2f2xuqra34p9pswlrq86nq63hna7p4vkrcrxznqslkta9eqs2nscfavlf"; // replace ... with the expected encoding

        // Act
        var result = Bech32.Encode(hrp, data);
        var decodedResult = Bech32.Decode(result, out var witVer, out var decodedHrp);

        // Assert
        Assert.Equal(expectedBech32String, result);
        Assert.Equal(hrp, decodedHrp);
        Assert.True(data.SequenceEqual(decodedResult));
    }
}
