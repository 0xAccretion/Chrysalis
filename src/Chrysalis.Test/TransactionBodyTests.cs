using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class TransactionBodyTests
{
    [Fact]
    public void TestTransactionBodyFromToCbor()
    {
        // Given
        // Replace the hex string with your real hex-encoded CBOR data for a TransactionBody
        string originalHex = "A40085825820375AAF2A056D060B18B9F4B23233750B7BB7E5BCA8655ECEDA87F2F76E542053008258200312AE1EE8A8756179A9E1844E3E591CB15114AA1F9C9BEA181AC838F82E94FE00825820B5E787BD2BBABD34CB4F5D72F092121B7A32992576D45BC5ED8C451B63FAFC7A008258209561256F19EEB08AF7C29344B1157B112C33663096ADAA14E5A4F009D1DD2348008258209561256F19EEB08AF7C29344B1157B112C33663096ADAA14E5A4F009D1DD234801018282583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F45450182583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A71A00DF2AEC021A0002BD7D031A0636E8BA";
        byte[] originalCborData = Convert.FromHexString(originalHex);

        // When (Deserialization)
        var deserializedTransactionBody = CborSerializer.Deserialize<TransactionBody>(originalCborData);

        // Prepare expected values
        // ... Initialize your expected values for Inputs, Outputs, and Fee
        // E.g.,
        var expectedFee = new CoinValue(179581UL);
        var expeectedTtl = 104261818UL;
        // var expectedInputs = ...
        // var expectedOutputs = ...

        // Then (Validating deserialization)
        Assert.Equal(expectedFee, deserializedTransactionBody.Fee);
        Assert.Equal(expeectedTtl, deserializedTransactionBody.TTL);
        // Assert.True(expectedInputs.SequenceEqual(deserializedTransactionBody.Inputs));
        // Assert.True(expectedOutputs.SequenceEqual(deserializedTransactionBody.Outputs));

        // When (Serialization)
        byte[] serializedCborData = CborSerializer.Serialize(deserializedTransactionBody);

        // Then (Validating serialization)
        Assert.True(originalCborData.SequenceEqual(serializedCborData));
    }
}
