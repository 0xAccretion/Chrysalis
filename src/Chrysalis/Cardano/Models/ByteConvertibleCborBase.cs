using System.Formats.Cbor;
using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models
{
    /// <summary>
    /// Base class for objects that can be converted to and from CBOR-encoded byte arrays.
    /// </summary>
    public abstract class ByteConvertibleCborBase : ByteConvertibleBase, ICborObject<ByteConvertibleCborBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteConvertibleCborBase"/> class with the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to initialize the object with.</param>
        public ByteConvertibleCborBase(byte[] bytes) : base(bytes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteConvertibleCborBase"/> class with the specified hexadecimal string.
        /// </summary>
        /// <param name="hexString">The hexadecimal string to initialize the object with.</param>
        public ByteConvertibleCborBase(string hexString) : base(hexString)
        {
        }

        /// <summary>
        /// Converts a CBOR-encoded byte array to an instance of the <see cref="ByteConvertibleCborBase"/> class.
        /// </summary>
        /// <param name="data">The CBOR-encoded byte array to convert.</param>
        /// <returns>An instance of the <see cref="ByteConvertibleCborBase"/> class.</returns>
        public abstract ByteConvertibleCborBase FromCbor(byte[] data);

        /// <summary>
        /// Converts a CBOR-encoded byte array to an instance of the <see cref="ByteConvertibleCborBase"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="CborReader"/> to read the CBOR-encoded data from.</param>
        /// <returns>An instance of the <see cref="ByteConvertibleCborBase"/> class.</returns>
        public abstract ByteConvertibleCborBase FromCbor(CborReader reader);

        /// <summary>
        /// Converts the object to a CBOR-encoded byte array.
        /// </summary>
        /// <returns>A CBOR-encoded byte array.</returns>
        public abstract byte[] ToCbor();

        /// <summary>
        /// Writes the object to the specified <see cref="CborWriter"/> as a CBOR-encoded byte array.
        /// </summary>
        /// <param name="writer">The <see cref="CborWriter"/> to write the CBOR-encoded data to.</param>
        /// <returns>The <see cref="CborWriter"/> instance.</returns>
        public abstract CborWriter ToCbor(CborWriter writer);

        /// <summary>
        /// Returns a hash code for the object.
        /// </summary>
        /// <returns>A hash code for the object.</returns>
        public override int GetHashCode()
        {
            byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(ToCbor());
            return BitConverter.ToInt32(hashBytes.Take(4).ToArray());
        }
    }
}
