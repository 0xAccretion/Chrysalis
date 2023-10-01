using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models
{
    /// <summary>
    /// Represents a collection of transaction outputs in a Cardano transaction.
    /// </summary>
    public class TransactionOutputs
    {
        // private readonly List<TransactionOutput> _outputs = new List<TransactionOutput>();

        // /// <summary>
        // /// Initializes a new instance of the <see cref="TransactionOutputs"/> class with an empty list of outputs.
        // /// </summary>
        // public TransactionOutputs() : base(Array.Empty<byte>()) { }

        // /// <summary>
        // /// Initializes a new instance of the <see cref="TransactionOutputs"/> class from a hex string.
        // /// </summary>
        // /// <param name="hex">The hex string to initialize the outputs from.</param>
        // public TransactionOutputs(string hex) : base(Array.Empty<byte>())
        // {
        //     FromCbor(Convert.FromHexString(hex));
        // }

        // /// <summary>
        // /// Initializes a new instance of the <see cref="TransactionOutputs"/> class from a CBOR-encoded byte array.
        // /// </summary>
        // /// <param name="cborData">The CBOR-encoded byte array to initialize the outputs from.</param>
        // public TransactionOutputs(byte[] cborData) : base(Array.Empty<byte>())
        // {
        //     FromCbor(cborData);
        // }

        // /// <inheritdoc/>
        // public override TransactionOutputs FromCbor(byte[] data)
        // {
        //     var reader = new CborReader(data);
        //     FromCbor(reader);
        //     return this;
        // }

        // /// <inheritdoc/>
        // public override TransactionOutputs FromCbor(CborReader reader)
        // {
        //     reader.ReadStartArray();
        //     _outputs.Clear();

        //     while (reader.PeekState() != CborReaderState.EndArray)
        //     {
        //         var output = new TransactionOutput(); // Assuming TransactionOutput has a method to read from CBOR
        //         output.FromCbor(reader);
        //         _outputs.Add(output);
        //     }

        //     reader.ReadEndArray();
        //     return this;
        // }

        // /// <inheritdoc/>
        // public override byte[] ToCbor()
        // {
        //     var writer = new CborWriter(CborConformanceMode.Strict);
        //     return ToCbor(writer).Encode();
        // }

        // /// <inheritdoc/>
        // public override CborWriter ToCbor(CborWriter writer)
        // {
        //     writer.WriteStartArray(_outputs.Count);

        //     foreach (var output in _outputs)
        //     {
        //         output.ToCbor(writer); // Assuming TransactionOutput has a method to write to CBOR
        //     }

        //     writer.WriteEndArray();
        //     return writer;
        // }

        // /// <inheritdoc/>
        // public override byte[] ToByteArray()
        // {
        //     return ToCbor();
        // }

        // /// <inheritdoc/>
        // public override string ToHexString()
        // {
        //     return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
        // }
    }
}