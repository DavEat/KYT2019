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
        Gear,
        ElectricComposent,
        _3DPrint,
        SolarPanel,
    }

    public readonly static string[] GOODS_NAMES =
    {
        "Sky Trash",
        "Iron Shard",
        "Plastic Shard",
        "Iron Ingot",
        "Plastic Wire",
        "Metal Gear",
        "Electric Composent",
        "3D Prints",
        "Solar Panel",
    };

    public GoodsType type;
    public int quantity;
    public int price;

    public Goods(GoodsType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
}
