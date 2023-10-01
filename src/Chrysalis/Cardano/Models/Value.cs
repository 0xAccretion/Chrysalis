using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;

public interface IValue
{
    ulong Coin { get; set; }
}

[CborType(CborRepresentation.UInt64, true)]
public class CoinValue : IValue
{
    public CoinValue()
    {
        Coin = 0;
    }
    
    public CoinValue(ulong coin)
    {
        Coin = coin;
    }

    public ulong Coin
    {
        get => Value;
        set => Value = value;
    }

    public ulong Value { get; set; }
}

[CborType(CborRepresentation.Tuple)]
public class MultiAssetValue : IValue
{
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.UInt64)]
    public ulong Coin { get; set; }

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Map)]
    public MultiAsset MultiAsset { get; set; }

    public MultiAssetValue()
    {
        Coin = 0;
        MultiAsset = [];
    }

    public MultiAssetValue(ulong coin, MultiAsset multiAsset)
    {
        if (multiAsset != null)
        {
            Coin = coin;
            MultiAsset = multiAsset;
        }
        else
        {
            throw new ArgumentNullException(nameof(multiAsset));
        }
    }
}