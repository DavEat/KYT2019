using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goods
{
    public enum GoodsType
    {
        SkyTrash,
        IronShard,
        PlasticShard,
        IronIngot,
        PlasticWire,
    }

    public GoodsType type;
    public int quantity;
    public int price;

    public Goods(GoodsType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
}
