using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfo : MonoBehaviour
{
    [SerializeField] GameObject m_infos = null;
    [SerializeField] TextMeshProUGUI m_title= null;
    [SerializeField] TextMeshProUGUI m_description= null;
    [Header("needs")]
    [SerializeField] TextMeshProUGUI m_need_r0_name= null;
    [SerializeField] TextMeshProUGUI m_need_r0_sell = null;
    [SerializeField] TextMeshProUGUI m_need_r0_stock = null;
    [SerializeField] TextMeshProUGUI m_need_r1_name= null;
    [SerializeField] TextMeshProUGUI m_need_r1_sell = null;
    [SerializeField] TextMeshProUGUI m_need_r1_stock = null;
    [SerializeField] TextMeshProUGUI m_need_r2_name= null;
    [SerializeField] TextMeshProUGUI m_need_r2_sell = null;
    [SerializeField] TextMeshProUGUI m_need_r2_stock = null;
    [SerializeField] GameObject m_need_none= null;

    [Header("productions")]
    [SerializeField] TextMeshProUGUI m_prod_r0_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r0_sell= null;
    [SerializeField] TextMeshProUGUI m_prod_r0_stock = null;
    [SerializeField] TextMeshProUGUI m_prod_r1_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r1_sell = null;
    [SerializeField] TextMeshProUGUI m_prod_r1_stock = null;
    [SerializeField] TextMeshProUGUI m_prod_r2_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r2_sell = null;
    [SerializeField] TextMeshProUGUI m_prod_r2_stock = null;
    [SerializeField] Toggle m_prod_sell_prod = null;
    [SerializeField] GameObject m_prod_none= null;

    public void DisplayItemInfos(string name, string desc, UIResourceData[] needs, UIResourceData[] prods, bool sellprod, bool warningAccess)
    {
        m_title.text = name;
        m_description.text = desc;

        if (needs != null)
        {
            if (needs.Length <= 0)
                m_need_none.SetActive(true);
            else m_need_none.SetActive(false);

            if (needs.Length > 0)
            {
                m_need_r0_name.gameObject.SetActive(true);
                m_need_r0_name.text = needs[0].name + ": " + needs[0].quantity;
                m_need_r0_sell.text = SellText(needs[0].sellPrice);
                m_need_r0_stock.text = StockText(needs[0].stock);
            }
            else m_need_r0_name.gameObject.SetActive(false);

            if (needs.Length > 1)
            {
                m_need_r1_name.gameObject.SetActive(true);
                m_need_r1_name.text = needs[1].name + ": " + needs[1].quantity;
                m_need_r1_sell.text = SellText(needs[1].sellPrice);
                m_need_r1_stock.text = StockText(needs[1].stock);
            }
            else m_need_r1_name.gameObject.SetActive(false);

            if (needs.Length > 2)
            {
                m_need_r2_name.gameObject.SetActive(true);
                m_need_r2_name.text = needs[2].name + ": " + needs[2].quantity;
                m_need_r2_sell.text = SellText(needs[2].sellPrice);
                m_need_r2_stock.text = StockText(needs[2].stock);
            }
            else m_need_r2_name.gameObject.SetActive(false);
        }
        else
        {
            m_need_r0_name.gameObject.SetActive(false);
            m_need_r1_name.gameObject.SetActive(false);
            m_need_r2_name.gameObject.SetActive(false);

            m_need_none.SetActive(true);
        }

        if (prods != null)
        {
            if (prods.Length <= 0)
            {
                m_prod_none.SetActive(true);
                m_prod_sell_prod.gameObject.SetActive(false);
            }
            else
            {
                m_prod_none.SetActive(false);
                m_prod_sell_prod.gameObject.SetActive(true);
                m_prod_sell_prod.isOn = sellprod;
            }

            if (prods.Length > 0)
            {
                m_prod_r0_name.gameObject.SetActive(true);
                m_prod_r0_name.text = prods[0].name + ": " + prods[0].quantity;
                m_prod_r0_sell.text = SellText(prods[0].sellPrice);
                m_prod_r0_stock.text = StockText(prods[0].stock);
            }
            else m_prod_r0_name.gameObject.SetActive(false);

            if (prods.Length > 1)
            {
                m_prod_r1_name.gameObject.SetActive(true);
                m_prod_r1_name.text = prods[1].name + ": " + prods[1].quantity;
                m_prod_r1_sell.text = SellText(prods[1].sellPrice);
                m_prod_r1_stock.text = StockText(prods[1].stock);
            }
            else m_prod_r1_name.gameObject.SetActive(false);

            if (prods.Length > 2)
            {
                m_prod_r2_name.gameObject.SetActive(true);
                m_prod_r2_name.text = prods[2].name + ": " + prods[2].quantity;
                m_prod_r2_sell.text = SellText(prods[2].sellPrice);
                m_prod_r2_stock.text = StockText(prods[2].stock);
            }
            else m_prod_r2_name.gameObject.SetActive(false);
        }
        else
        {
            m_prod_r0_name.gameObject.SetActive(false);
            m_prod_r1_name.gameObject.SetActive(false);
            m_prod_r2_name.gameObject.SetActive(false);

            m_prod_none.SetActive(true);
            m_prod_sell_prod.gameObject.SetActive(false);
        }

        m_infos.SetActive(true);
    }

    public void UpdateStockInfo(UIResourceData[] needs, UIResourceData[] prods)
    {
        if (needs != null)
        {
            if (needs.Length > 0)
                m_need_r0_stock.text = StockText(needs[0].stock);

            if (needs.Length > 1)
                m_need_r1_stock.text = StockText(needs[1].stock);

            if (needs.Length > 2)
                m_need_r2_stock.text = StockText(needs[2].stock);
        }
        if (prods != null)
        {
            if (prods.Length > 0)
                m_prod_r0_stock.text = StockText(prods[0].stock);

            if (prods.Length > 1)
                m_prod_r1_stock.text = StockText(prods[1].stock);

            if (prods.Length > 2)
                m_prod_r2_stock.text = StockText(prods[2].stock);
        }
    }

    public void HideItemInfos()
    {
        m_infos.SetActive(false);
    }

    public struct UIResourceData
    {
        public string name;
        public int quantity;
        public int sellPrice;
        public int stock;

        public UIResourceData(string name, int quantity, int stock)
        {
            this.name = name;
            this.quantity = quantity;
            this.sellPrice = -1;
            this.stock = stock;
        }
        public UIResourceData(string name, int quantity, int sellPrice, int stock)
        {
            this.name = name;
            this.quantity = quantity;
            this.sellPrice = sellPrice;
            this.stock = stock;
        }
    }

    public void SellProductionToogle()
    {
        if (SelectionManager.selection is Building)
        {
            ((Building)SelectionManager.selection).m_sellProduction = m_prod_sell_prod.isOn;
        }
    }

    string SellText(int value)
    {
        return string.Format("(sell: {0}", value);
    }
    string StockText(int value)
    {
        return string.Format("stock: {0})", value);
    }
}
