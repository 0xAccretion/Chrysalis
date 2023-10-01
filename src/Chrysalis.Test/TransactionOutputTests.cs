using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class TransactionOutputTests
{
    private const string OriginalHex = "82583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501";  // Replace with the original Hex representation
    private static readonly byte[] OriginalCborData = Convert.FromHexString(OriginalHex);

    [Fact]
    public void TestTransactionOutputFromToCbor()
    {
        // When (Deserialization)
        var transactionOutput = CborSerializerV2.Deserialize<TransactionOutput>(OriginalCborData);

        // Prepare expected values
        var expectedAddress = "YourExpectedAddressHere"; // Replace with the expected address
        var expectedAmount = new CoinValue { Coin = 42 }; // Replace with the expected amount

        // Then
        Assert.Equal(expectedAddress, transactionOutput!.Address);
        Assert.Equal(expectedAmount.Coin, transactionOutput.Amount.Coin);

        // When (Serialization)
        byte[] serializedCborData = CborSerializerV2.Serialize(transactionOutput);

        // Then
        Assert.True(OriginalCborData.SequenceEqual(serializedCborData));
    }

    [Fact]
    public void TestTransactionOutputFromToHex()
    {
        // When
        TransactionOutput originalTransactionOutput = CborSerializerV2.FromHex<TransactionOutput>(OriginalHex)!;
        string convertedHex = CborSerializerV2.ToHex(originalTransactionOutput);

        // Then
        Assert.Equal(OriginalHex.ToLowerInvariant(), convertedHex);
    }

    [Fact]
    public void TestTransactionOutputFromToBytes()
    {
        // When
        TransactionOutput originalTransactionOutput = CborSerializerV2.Deserialize<TransactionOutput>(OriginalCborData)!;
        byte[] convertedBytes = CborSerializerV2.Serialize(originalTransactionOutput);

        // Then
        Assert.True(OriginalCborData.SequenceEqual(convertedBytes));
    }

    // [Fact]
    // public void TestTransactionOutputFromToCbor()
    // {
    //     // Given
    //     byte[] originalCborData = Convert.FromHexString("82583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501");

    //     // When (Deserialization)
    //     var transactionOutput = CborSerializer.Deserialize<TransactionOutput>(originalCborData);

    //     // Prepare expected values
    //     var expectedAddress = new ByteString("01e63022b0f461602484968bb10fd8f872787b862ace2d7e943292a37003ec6a12860ef8c07d4c1a8de7df06acb0f0330a6087ecbe972082a7");
    //     var expectedAmount = new CoinValue(1_452_470);  // Replace with your expected amount
    //     // var expectedDatumHash = new ByteString("Your_Expected_DatumHash_Bytes_Here");

    //     // Then (Validating deserialization)
    //     Assert.Equal(expectedAddress, transactionOutput.Address);
    //     Assert.Equal(expectedAmount, transactionOutput.Amount);
    //     // Assert.Equal(expectedDatumHash, transactionOutput.DatumHash);

    //     // When (Serialization)
    //     byte[] serializedCborData = CborSerializer.Serialize(transactionOutput);

    //     // Then (Validating serialization)
    //     Assert.True(originalCborData.SequenceEqual(serializedCborData));
    // }

    // [Fact]
    // public void TestTransactionOutputFromToHex()
    // {
    //     // Given
    //     string originalHex = "82583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501";
    //     var originalTransactionOutput = ByteConvertibleFactory.FromHex<TransactionOutput>(originalHex);

    //     // When
    //     string convertedHex = originalTransactionOutput?.ToHexString()!;

    //     // Then
    //     Assert.Equal(originalHex.ToLowerInvariant(), convertedHex);
    // }

    // [Fact]
    // public void TestTransactionOutputFromToBytes()
    // {
    //     // Given
    //     byte[] originalBytes = Convert.FromHexString("82583901E63022B0F461602484968BB10FD8F872787B862ACE2D7E943292A37003EC6A12860EF8C07D4C1A8DE7DF06ACB0F0330A6087ECBE972082A7821A001629B6A1581C6F37A98BD0C9CED4E302EC2FB3A2F19FFBA1B5C0C2BEDEE3DAC30E56A45148595045534B554C4C535F56545F505F45015148595045534B554C4C535F56545F565F43015248595045534B554C4C535F56545F4D5F4545015348595045534B554C4C535F56545F41435F454501");
    //     var originalTransactionOutput = ByteConvertibleFactory.FromBytes<TransactionOutput>(originalBytes);

    //     // When
    //     byte[]? convertedBytes = originalTransactionOutput?.ToByteArray()!;

    //     // Then
    //     Assert.True(originalBytes.SequenceEqual(convertedBytes));
    // }
}
