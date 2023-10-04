using Chrysalis.Cbor;

namespace Chrysalis.Cardano.Models;


[CborType(CborRepresentation.UInt64, true)]
[CborType(CborRepresentation.Tuple)]
public class Value
{

    public Value()
    {
        Coin = 0;
        MultiAsset = [];
    }
    
    public Value(ulong coin)
    {
        CborValue = coin;
        MultiAsset = [];
    }

    public Value(ulong coin, MultiAsset multiAsset)
    {
        Coin = coin;
        MultiAsset = multiAsset;
    }

    private ulong _coin = 0;
    [CborProperty(CborRepresentation.Int32, 0, CborRepresentation.UInt64)]
    public ulong Coin
    {
        get => CborValue ?? _coin;
        set
        {
            if(CborValue is null)
            {
                _coin = value;
            }
            else
            {
                CborValue = value;
            }
        }
    }

    [CborProperty(CborRepresentation.Int32, 1, CborRepresentation.Map)]
    public MultiAsset MultiAsset { get; set; }

    public ulong? CborValue { get; set; }
}
