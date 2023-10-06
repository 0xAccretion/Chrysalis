namespace Chrysalis.Cardano.Models;

public enum NetworkTag
{
    Unknown = -1,
    Testnet = 0x00, // ....0000
    Mainnet = 0x01  // ....0001
    // Add other network tags as needed
}

public enum AddressType
{
    Unknown = -1,
    PaymentKeyHash_StakeKeyHash = 0x00, // 0000....
    ScriptHash_StakeKeyHash = 0x10,     // 0001....
    PaymentKeyHash_ScriptHash = 0x20,   // 0010....
    ScriptHash_ScriptHash = 0x30,       // 0011....
    PaymentKeyHash_Pointer = 0x40,      // 0100....
    ScriptHash_Pointer = 0x50,          // 0101....
    PaymentKeyHash = 0x60,              // 0110....
    ScriptHash = 0x70                   // 0111....
    // Add other Shelley address types as needed
}

public class Address
{
    private const byte HeaderTypeMask = 0xF0;
    private const byte NetworkTagMask = 0x0F;

    public byte Header { get; private set; }
    public byte[] Payload { get; private set; }
    private byte _headerType => (byte)(Header & HeaderTypeMask);
    private byte _networkTag => (byte)(Header & NetworkTagMask);
    public NetworkTag NetworkTag => Enum.IsDefined(typeof(NetworkTag), (int)_networkTag) ?
        (NetworkTag)_networkTag : NetworkTag.Unknown;

    public AddressType AddressType => Enum.IsDefined(typeof(AddressType), (int)_headerType) ?
        (AddressType)_headerType : AddressType.Unknown;

    private Address(byte header, byte[] payload)
    {
        Header = header;
        Payload = payload;
    }

    public static string ToHex(Address address)
    {
        Span<byte> addressBytes = stackalloc byte[address.Payload.Length + 1];  // Allocate memory on the stack
        addressBytes[0] = address.Header;
        address.Payload.AsSpan().CopyTo(addressBytes[1..]);  // Copy data without extra allocations

        return Convert.ToHexString(addressBytes);
    }

    public static Address FromHex(string hex)
    {
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string should have an even length", nameof(hex));

        // Convert hex to byte array
        byte[] addressBytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            addressBytes[i / 2] = byte.Parse(hex.AsSpan(i, 2).ToString(), System.Globalization.NumberStyles.HexNumber);
        }

        // Use Span for slicing without new array allocations
        byte header = addressBytes[0];
        byte[] payload = addressBytes.AsSpan(1).ToArray();

        return new Address(header, payload);
    }

    public byte[] GetPaymentKeyHash()
    {
        if (Payload.Length < 28) // Validate if the payload is long enough
        {
            throw new InvalidOperationException("Payload is not long enough to contain a PaymentKeyHash.");
        }

        return AddressType switch
        {
            AddressType.PaymentKeyHash_StakeKeyHash or AddressType.ScriptHash_StakeKeyHash or AddressType.PaymentKeyHash_ScriptHash or AddressType.ScriptHash_ScriptHash or AddressType.PaymentKeyHash_Pointer or AddressType.ScriptHash_Pointer or AddressType.PaymentKeyHash or AddressType.ScriptHash
              => Payload.AsSpan(0, 28).ToArray(),
            _ => throw new InvalidOperationException($"Address type {AddressType} does not contain a PaymentKeyHash."),
        };
    }

    public byte[] GetStakeKeyHash()
    {
        if (Payload.Length < 56) // Validate if the payload is long enough
        {
            throw new InvalidOperationException("Payload is not long enough to contain a StakeKeyHash.");
        }

        return AddressType switch
        {
            AddressType.PaymentKeyHash_StakeKeyHash or AddressType.ScriptHash_StakeKeyHash or AddressType.PaymentKeyHash_ScriptHash or AddressType.ScriptHash_ScriptHash
              => Payload.AsSpan(28, 28).ToArray(),
            _ => throw new InvalidOperationException($"Address type {AddressType} does not contain a StakeKeyHash."),
        };
    }
}

