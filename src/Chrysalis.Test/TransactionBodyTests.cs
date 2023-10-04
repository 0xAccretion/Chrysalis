using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class TransactionBodyTests
{
    public static readonly string originHex = "A40085825820375AAF2A056D060B18B9F4B23233750B7BB7E5BCA8655ECEDA87F2F76E542053008258200312AE1EE8A8756179A9E1844E3E591CB15114AA1F9C9BEA181AC838F82E94FE00825820B5E787BD2BBABD34CB4F5D72F092121B7A32992576D45BC5ED8C451B63FAFC7A008258209561256F19EEB08AF7C29344B1157B112C33663096ADAA14E5A4F009D1DD2348008258209561256F19EEB08AF7C29344B1157B112C33663096ADAA14E5A4F009D1DD234801018282583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F45450182583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A71A00DF2AEC021A0002BD7D031A0636E8BA";
    public static readonly string expectedInputsCborHex = "85825820375aaf2a056d060b18b9f4b23233750b7bb7e5bca8655eceda87f2f76e542053008258200312ae1ee8a8756179a9e1844e3e591cb15114aa1f9c9bea181ac838f82e94fe00825820b5e787bd2bbabd34cb4f5d72f092121b7a32992576d45bc5ed8c451b63fafc7a008258209561256f19eeb08af7c29344b1157b112c33663096adaa14e5a4f009d1dd2348008258209561256f19eeb08af7c29344b1157b112c33663096adaa14e5a4f009d1dd234801";
    public static readonly string expectedOutputsCborHex = "8282583901e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a37003ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a7821a001629b6a1581c6f37a98bd0c9ced4e302ec2fb3a2f19ffba1b5c0c2bedee3dac30e56a45148595045534b554c4c535f56545f505f45015148595045534b554c4c535f56545f565f43015248595045534b554c4c535f56545f4d5f4545015348595045534b554c4c535f56545f41435f45450182583901e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a37003ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a71a00df2aec";
    
    [Fact]
    public void TestTransactionBodyFromToCbor()
    {
        // Given
        // Replace the hex string with your real hex-encoded CBOR data for a TransactionBody
        byte[] originalCborData = Convert.FromHexString(originHex);

        // When (Deserialization)
        var deserializedTransactionBody = CborSerializerV2.Deserialize<TransactionBody>(originalCborData);

        // Prepare expected values
        // Initialize your expected values for Inputs, Outputs, and Fee
        // var expectedFee = new CoinValue(179581UL); // Replace with your real expected value
        // var expectedTtl = 104261818UL; // Replace with your real expected value
        // var expectedInputs = ... // Replace with your real expected value
        // var expectedOutputs = ... // Replace with your real expected value

        // // Then (Validating deserialization)
        // Assert.Equal(expectedFee, deserializedTransactionBody.Fee);
        // Assert.Equal(expectedTtl, deserializedTransactionBody.TTL);

        // Uncomment and update these lines as necessary:
        // Assert.True(expectedInputs.SequenceEqual(deserializedTransactionBody.Inputs));
        // Assert.True(expectedOutputs.SequenceEqual(deserializedTransactionBody.Outputs));

        // When (Serialization)
        byte[] serializedCborData = CborSerializerV2.Serialize(deserializedTransactionBody!);

        // Then (Validating serialization)
        Assert.True(originalCborData.SequenceEqual(serializedCborData));
    }
}
