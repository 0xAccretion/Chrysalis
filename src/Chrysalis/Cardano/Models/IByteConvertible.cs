namespace Chrysalis.Cardano.Models;

public interface IByteConvertible
{
    byte[] ToByteArray();
    string ToHexString();
}