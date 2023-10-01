using Chrysalis.Cbor;
using Chrysalis.Cardano.Models;

namespace Chrysalis.Test
{
    public class TransactionInputsTests
    {
        [Fact]
        public void TestTransactionInputsFromToCbor()
        {
            // Arrange
            var transactionInput1 = new TransactionInput
            {
                TransactionId = "e33b1a20c078149fde522803ef0b6a50b672e60765aaf6fde5d15896ca3f8626",
                Index = 0
            };

            var transactionInput2 = new TransactionInput
            {
                TransactionId = "e33b1a20c078149fde522803ef0b6a50b672e60765aaf6fde5d15896ca3f8625",
                Index = 1
            };

            var originalTransactionInputs = new TransactionInputs
            {
                transactionInput1, 
                transactionInput2
            };

            // Act
            // Serialization
            byte[] serializedCborData = CborSerializerV2.Serialize(originalTransactionInputs);

            // Deserialization
            var deserializedTransactionInputs = CborSerializerV2.Deserialize<TransactionInputs>(serializedCborData);

            // Assert
            Assert.NotNull(deserializedTransactionInputs);
            Assert.Equal(2, deserializedTransactionInputs.Count);
            Assert.Contains(transactionInput1, deserializedTransactionInputs);
            Assert.Contains(transactionInput2, deserializedTransactionInputs);
        }
    }
}
