using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models
{
    /// <summary>
    /// Represents a collection of transaction inputs.
    /// </summary>
    public class TransactionInputs : ByteConvertibleCborBase, ICborObject<TransactionInputs>
    {
        private readonly HashSet<TransactionInput> _transactionInputs = new HashSet<TransactionInput>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionInputs"/> class with an empty collection of inputs.
        /// </summary>
        public TransactionInputs() : base(Array.Empty<byte>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionInputs"/> class from a hex string.
        /// </summary>
        /// <param name="hex">The hex string to initialize the inputs from.</param>
        public TransactionInputs(string hex) : base(Array.Empty<byte>())
        {
            FromCbor(Convert.FromHexString(hex));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionInputs"/> class from a CBOR-encoded byte array.
        /// </summary>
        /// <param name="cborData">The CBOR-encoded byte array to initialize the inputs from.</param>
        public TransactionInputs(byte[] cborData) : base(Array.Empty<byte>())
        {
            FromCbor(cborData);
        }

        /// <inheritdoc/>
        public override TransactionInputs FromCbor(byte[] data)
        {
            var reader = new CborReader(data);
            FromCbor(reader);
            return this;
        }

        /// <inheritdoc/>
        public override TransactionInputs FromCbor(CborReader reader)
        {
            reader.ReadStartArray();

            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var transactionInput = new TransactionInput().FromCbor(reader);
                if (transactionInput != null)
                {
                    _transactionInputs.Add(transactionInput);
                }
            }

            reader.ReadEndArray();
            return this;
        }

        /// <inheritdoc/>
        public override byte[] ToCbor()
        {
            var writer = new CborWriter(CborConformanceMode.Strict);
            return ToCbor(writer).Encode();
        }

        /// <inheritdoc/>
        public override CborWriter ToCbor(CborWriter writer)
        {
            writer.WriteStartArray(_transactionInputs.Count);

            foreach (var input in _transactionInputs)
            {
                input.ToCbor(writer);
            }

            writer.WriteEndArray();
            return writer;
        }

        /// <inheritdoc/>
        public override byte[] ToByteArray()
        {
            return ToCbor();
        }

        /// <inheritdoc/>
        public override string ToHexString()
        {
            return Convert.ToHexString(ToByteArray()).ToLowerInvariant();
        }
    }
}