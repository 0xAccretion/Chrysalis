/*
 * This implementation of the Bech32 encoder/decoder is based on and inspired by two open-source projects:
 * 1. Bech32 library by lontivero
 *    Source: https://github.com/lontivero/Bech32/blob/master/Bech32/Bech32.cs
 * 
 * 2. CardanoSharp Wallet library
 *    Source: https://github.com/CardanoSharp/cardanosharp-wallet/blob/master/CardanoSharp.Wallet/Encoding/Bech32.cs
 * 
 * Thanks to the authors and contributors of these projects for their valuable work.
 */

using System.Buffers;
using System.Text;

namespace Chrysalis;

public static class Bech32
{
    private const string Charset = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";
    private const char Separator = '1';
    private static readonly uint[] Generator =
    {
            0x3b6a57b2u, 0x26508e6du, 0x1ea119fau, 0x3d4233ddu, 0x2a1462b3u
    };
    private const int checkSumSize = 6;

    private static uint Polymod(Span<byte> data)
    {
        uint chk = 1;
        foreach (byte b in data)
        {
            uint temp = chk >> 25;
            chk = ((chk & 0x1ffffff) << 5) ^ b;
            for (int i = 0; i < 5; i++)
            {
                if (((temp >> i) & 1) == 1)
                {
                    chk ^= Generator[i];
                }
            }
        }
        return chk;
    }

    private static byte[] ExpandHrp(string hrp)
    {
        var result = new byte[(2 * hrp.Length) + 1];
        for (int i = 0; i < hrp.Length; i++)
        {
            result[i] = (byte)(hrp[i] >> 5);
            result[i + hrp.Length + 1] = (byte)(hrp[i] & 0b0001_1111);
        }
        return result;
    }

    private static byte[] CalculateCheckSum(string hrp, Span<byte> data)
    {
        var values = ExpandHrp(hrp).AsSpan().ConcatFast(data).ConcatFast(new byte[6].AsSpan()).ToArray();
        var pm = Polymod(values.AsSpan()) ^ 1;
        var result = new byte[6];
        for (var i = 0; i < 6; i++)
        {
            result[i] = (byte)((pm >> 5 * (5 - i)) & 0b0001_1111);
        }
        return result;
    }

    public static string Encode(string hrp, Span<byte> data)
    {
        if (string.IsNullOrEmpty(hrp))
            throw new ArgumentNullException(nameof(hrp), "hrp cannot be null or empty.");
        if (data.IsEmpty)
            throw new ArgumentNullException(nameof(data), "Data cannot be empty.");

        var b32Arr = ConvertBits(data, 8, 5);
        var checksum = CalculateCheckSum(hrp, b32Arr);
        var result = new StringBuilder(hrp).Append(Separator);
        foreach (var b in b32Arr)
        {
            result.Append(Charset[b]);
        }
        foreach (var b in checksum)
        {
            result.Append(Charset[b]);
        }

        return result.ToString();
    }

    private static byte[] ConvertBits(Span<byte> data, int fromBits, int toBits, bool pad = true)
    {
        int acc = 0;
        int bits = 0;
        int maxv = (1 << toBits) - 1;
        int maxacc = (1 << (fromBits + toBits - 1)) - 1;

        var result = new ArrayBufferWriter<byte>();
        foreach (var b in data)
        {
            if ((b >> fromBits) != 0)
            {
                throw new FormatException("Invalid data format.");
            }
            acc = ((acc << fromBits) | b) & maxacc;
            bits += fromBits;
            while (bits >= toBits)
            {
                bits -= toBits;
                result.Write(new byte[] { (byte)((acc >> bits) & maxv) });
            }
        }
        if (pad)
        {
            if (bits > 0)
            {
                result.Write(new byte[] { (byte)((acc << (toBits - bits)) & maxv) });
            }
        }
        else if (bits >= fromBits || ((acc << (toBits - bits)) & maxv) != 0)
        {
            throw new FormatException("Invalid data format.");
        }
        return result.WrittenSpan.ToArray();
    }

    public static (string hrp, byte[] data) Decode(string bech32EncodedString)
    {
        byte[] b32Arr = Bech32Decode(bech32EncodedString, out string hrp);

        if (b32Arr.Length < checkSumSize)
        {
            throw new FormatException("Invalid data length.");
        }

        if (!VerifyChecksum(hrp, b32Arr))
        {
            throw new FormatException("Invalid checksum.");
        }

        byte[] dataPart = b32Arr.SubArray(0, b32Arr.Length - checkSumSize);
        byte[] b256Arr = ConvertBits(dataPart, 5, 8, false) ?? throw new FormatException("Invalid data format.");

        return (hrp, b256Arr);
    }


    private static byte[] Bech32Decode(string bech32EncodedString, out string hrp)
    {
        bech32EncodedString = bech32EncodedString.ToLower();

        int sepIndex = bech32EncodedString.LastIndexOf(Separator);
        hrp = bech32EncodedString[..sepIndex];
        string data = bech32EncodedString[(sepIndex + 1)..];

        byte[] b32Arr = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            b32Arr[i] = (byte)Charset.IndexOf(data[i]);
        }

        return b32Arr;
    }

    public static byte[] ConcatFast(this byte[] firstArray, byte[] secondArray)
    {
        if (firstArray == null)
            throw new ArgumentNullException(nameof(firstArray), "First array can not be null!");
        if (secondArray == null)
            throw new ArgumentNullException(nameof(secondArray), "Second array can not be null!");


        byte[] result = new byte[firstArray.Length + secondArray.Length];
        Buffer.BlockCopy(firstArray, 0, result, 0, firstArray.Length);
        Buffer.BlockCopy(secondArray, 0, result, firstArray.Length, secondArray.Length);
        return result;
    }

    public static byte[] SubArray(this byte[] sourceArray, int index, int count)
    {
        if (sourceArray == null)
            throw new ArgumentNullException(nameof(sourceArray), "Input can not be null!");
        if (index < 0 || count < 0)
            throw new IndexOutOfRangeException("Index or count can not be negative.");
        if (sourceArray.Length != 0 && index > sourceArray.Length - 1 || sourceArray.Length == 0 && index != 0)
            throw new IndexOutOfRangeException("Index can not be bigger than array length.");
        if (count > sourceArray.Length - index)
            throw new IndexOutOfRangeException("Array is not long enough.");


        byte[] result = new byte[count];
        Buffer.BlockCopy(sourceArray, index, result, 0, count);
        return result;
    }

    private static bool VerifyChecksum(string hrp, byte[] data)
    {
        byte[] temp = ExpandHrp(hrp).ConcatFast(data);
        return Polymod(temp) == 1;
    }
}

public static class SpanExtensions
{
    public static Span<byte> ConcatFast(this Span<byte> first, Span<byte> second)
    {
        byte[] result = new byte[first.Length + second.Length];
        first.CopyTo(result);
        second.CopyTo(result.AsSpan(first.Length));
        return result;
    }
}
