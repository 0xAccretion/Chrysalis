using Chrysalis.Cardano.Models;

namespace Chrysalis.Test;

public class AddressTest
{
    [Fact]
    public void Address_ToHex_FromHex_Test()
    {
        string expectedHex = "01e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a37003ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a7";

        // Decode
        Address address = Address.FromHex(expectedHex);

        // Encode
        string resultHex = Address.ToHex(address).ToLowerInvariant();

        // Assert
        Assert.Equal(NetworkTag.Mainnet, address.NetworkTag);
        Assert.Equal(AddressType.PaymentKeyHash_StakeKeyHash, address.AddressType);
        Assert.Equal(expectedHex, resultHex);
        Assert.Equal("e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a370", Convert.ToHexString(address.GetPaymentKeyHash()).ToLowerInvariant());
        Assert.Equal("03ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a7", Convert.ToHexString(address.GetStakeKeyHash()).ToLowerInvariant());
    }
}
