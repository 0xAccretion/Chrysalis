using Chrysalis.Cardano.Models;

namespace Chrysalis.Test;

public class AddressTest
{
    [Fact]
    public void Address_ToHex_FromHex_Test()
    {
        string expectedHex = "019493315cd92eb5d8c4304e67b7e16ae36d61d34502694657811a2c8e337b62cfff6403a06a3acbc34f8c46003c69fe79a3628cefa9c47251";

        // Decode
        Address address = Address.FromHex(expectedHex);

        // Encode
        string resultHex = Address.ToHex(address).ToLowerInvariant();

        // Assert
        Assert.Equal(NetworkTag.Mainnet, address.NetworkTag);
        Assert.Equal(AddressType.PaymentKeyHash_StakeKeyHash, address.AddressType);
        Assert.Equal(expectedHex, resultHex);
        Assert.Equal("9493315cd92eb5d8c4304e67b7e16ae36d61d34502694657811a2c8e", Convert.ToHexString(address.GetPaymentKeyHash()).ToLowerInvariant());
        Assert.Equal("337b62cfff6403a06a3acbc34f8c46003c69fe79a3628cefa9c47251", Convert.ToHexString(address.GetStakeKeyHash()).ToLowerInvariant());
    }


    [Theory]
    [InlineData("019493315cd92eb5d8c4304e67b7e16ae36d61d34502694657811a2c8e337b62cfff6403a06a3acbc34f8c46003c69fe79a3628cefa9c47251", "addr1qx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer3n0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgse35a3x")]
    public void Address_Bech32_Test(string expectedHex, string expectedBech32)
    {
        // Convert the Bech32 string to Address
        (_, var addressFromBech32) = Bech32.Decode(expectedBech32); // Assumes a Bech32.Decode implementation exists
        var address = Address.FromBytes(addressFromBech32);
        // Convert the Address object back to Bech32 string
        string resultBech32 = address.ToBech32();

        // Convert the Address object to its hex representation
        string resultHex = Address.ToHex(address).ToLowerInvariant();

        // Assert that the results match their expected values
        Assert.Equal(expectedBech32, resultBech32);
        Assert.Equal(expectedHex, resultHex);
    }

    [Theory]
    [InlineData("addr1qx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer3n0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgse35a3x")]
    [InlineData("addr1z8phkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gten0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgs9yc0hh")]
    [InlineData("addr1yx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzerkr0vd4msrxnuwnccdxlhdjar77j6lg0wypcc9uar5d2shs2z78ve")]
    [InlineData("addr1x8phkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gt7r0vd4msrxnuwnccdxlhdjar77j6lg0wypcc9uar5d2shskhj42g")]
    [InlineData("addr1gx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer5pnz75xxcrzqf96k")]
    [InlineData("addr128phkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtupnz75xxcrtw79hu")]
    [InlineData("addr1vx2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzers66hrl8")]
    [InlineData("addr1w8phkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtcyjy7wx")]
    [InlineData("stake1uyehkck0lajq8gr28t9uxnuvgcqrc6070x3k9r8048z8y5gh6ffgw")]
    [InlineData("stake178phkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtcccycj5")]
    [InlineData("addr_test1qz2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer3n0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgs68faae")]
    [InlineData("addr_test1zrphkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gten0d3vllmyqwsx5wktcd8cc3sq835lu7drv2xwl2wywfgsxj90mg")]
    [InlineData("addr_test1yz2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzerkr0vd4msrxnuwnccdxlhdjar77j6lg0wypcc9uar5d2shsf5r8qx")]
    [InlineData("addr_test1xrphkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gt7r0vd4msrxnuwnccdxlhdjar77j6lg0wypcc9uar5d2shs4p04xh")]
    [InlineData("addr_test1gz2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzer5pnz75xxcrdw5vky")]
    [InlineData("addr_test12rphkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtupnz75xxcryqrvmw")]
    [InlineData("addr_test1vz2fxv2umyhttkxyxp8x0dlpdt3k6cwng5pxj3jhsydzerspjrlsz")]
    [InlineData("addr_test1wrphkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtcl6szpr")]
    [InlineData("stake_test1uqehkck0lajq8gr28t9uxnuvgcqrc6070x3k9r8048z8y5gssrtvn")]
    [InlineData("stake_test17rphkx6acpnf78fuvxn0mkew3l0fd058hzquvz7w36x4gtcljw6kf")]
    public void Address_Bech32DecodeEncode_Test(string inputBech32)
    {
        // Decode the Bech32 string
        var (hrp, data) = Bech32.Decode(inputBech32);

        // Create an Address object from decoded data
        Address address = Address.FromBytes(data);

        // Encode the Address object back to Bech32
        string outputBech32 = address.ToBech32();

        // Assert
        Assert.Equal(inputBech32, outputBech32);
    }
}
