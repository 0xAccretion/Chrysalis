using Chrysalis.Cardano.Models;
using Chrysalis.Cbor;

namespace Chrysalis.Test;

public class CborSerializationV2Tests
{
    [Fact]
    public void TestExampleSerializationAndDeserialization()
    {
        // Arrange
        var original = new Example
        {
            TransactionId = "375aaf2a056d060b18b9f4b23233750b7bb7e5bca8655eceda87f2f76e542053",
            Index = 0
        };

        // Act
        byte[] serialized = CborSerializerV2.Serialize(original);
        var deserialized = CborSerializerV2.Deserialize<Example>(serialized);

        // Assert
        Assert.Equal(original.TransactionId, deserialized.TransactionId);
        Assert.Equal(original.Index, deserialized.Index);
    }

}
