using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;
namespace Chrysalis.Test;

public class TransactionInputTests
{
    [Fact]
    public void TestTransactionInputFromToCbor()
    {
        // Given
        byte[] originalCborData = Convert.FromHexString("825820E33B1A20C078149FDE522803EF0B6A50B672E60765AAF6FDE5D15896CA3F862600");

        // When (Deserialization)
        var transactionInput = CborSerializer.Deserialize<TransactionInput>(originalCborData);

        // Prepare expected TransactionId
        var expectedTransactionId = new ByteString("e33b1a20c078149fde522803ef0b6a50b672e60765aaf6fde5d15896ca3f8626");

        // Then
        Assert.Equal(expectedTransactionId, transactionInput.TransactionId);
        Assert.Equal(0, transactionInput.Index);

        // When (Serialization)
        byte[] serializedCborData = CborSerializer.Serialize(transactionInput);

        // Then
        Assert.Equal(originalCborData, serializedCborData);
    }

    [Fact]
    public void TestTransactionInputFromToHex()
    {
        // Given
        string originalHex = "825820E33B1A20C078149FDE522803EF0B6A50B672E60765AAF6FDE5D15896CA3F862600";
        TransactionInput? originalTransactionInput = ByteConvertibleFactory.FromHex<TransactionInput>(originalHex);

        // When
        string convertedHex = originalTransactionInput?.ToHexString()!;

        // Then
        Assert.Equal(originalHex.ToLowerInvariant(), convertedHex);
    }

    [Fact]
    public void TestTransactionInputFromToBytes()
    {
        // Given
        byte[] originalBytes = Convert.FromHexString("825820E33B1A20C078149FDE522803EF0B6A50B672E60765AAF6FDE5D15896CA3F862600");
        TransactionInput? originalTransactionInput = ByteConvertibleFactory.FromBytes<TransactionInput>(originalBytes);

        // When
        byte[]? convertedBytes = originalTransactionInput?.ToByteArray()!;

        // Then
        Assert.True(originalBytes.SequenceEqual(convertedBytes));
    }
}