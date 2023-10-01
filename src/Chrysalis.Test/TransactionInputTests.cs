using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test
{
    public class TransactionInputTests
    {
        private const string OriginalHex = "825820E33B1A20C078149FDE522803EF0B6A50B672E60765AAF6FDE5D15896CA3F862600";
        private static readonly byte[] OriginalCborData = Convert.FromHexString(OriginalHex);

        [Fact]
        public void TestTransactionInputFromToCbor()
        {
            // When (Deserialization)
            var transactionInput = CborSerializerV2.Deserialize<TransactionInput>(OriginalCborData);

            // Prepare expected TransactionId
            var expectedTransactionId = "e33b1a20c078149fde522803ef0b6a50b672e60765aaf6fde5d15896ca3f8626";

            // Then
            Assert.Equal(expectedTransactionId, transactionInput!.TransactionId);
            Assert.Equal(0u, transactionInput.Index);

            // When (Serialization)
            byte[] serializedCborData = CborSerializerV2.Serialize(transactionInput);

            // Then
            Assert.True(OriginalCborData.SequenceEqual(serializedCborData));
        }

        [Fact]
        public void TestTransactionInputFromToHex()
        {
            // When
            TransactionInput originalTransactionInput = CborSerializerV2.FromHex<TransactionInput>(OriginalHex)!;
            string convertedHex = CborSerializerV2.ToHex(originalTransactionInput);

            // Then
            Assert.Equal(OriginalHex.ToLowerInvariant(), convertedHex);
        }

        [Fact]
        public void TestTransactionInputFromToBytes()
        {
            // When
            TransactionInput originalTransactionInput = CborSerializerV2.Deserialize<TransactionInput>(OriginalCborData)!;
            byte[] convertedBytes = CborSerializerV2.Serialize(originalTransactionInput);

            // Then
            Assert.True(OriginalCborData.SequenceEqual(convertedBytes));
        }
    }
}
