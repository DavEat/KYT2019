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
    [SerializeField] GameObject m_need_r0= null;
    [SerializeField] TextMeshProUGUI m_need_r0_name= null;
    [SerializeField] TextMeshProUGUI m_need_r0_quantity= null;
    [SerializeField] GameObject m_need_r1= null;
    [SerializeField] TextMeshProUGUI m_need_r1_name= null;
    [SerializeField] TextMeshProUGUI m_need_r1_quantity= null;
    [SerializeField] GameObject m_need_r2= null;
    [SerializeField] TextMeshProUGUI m_need_r2_name= null;
    [SerializeField] TextMeshProUGUI m_need_r2_quantity= null;
    [SerializeField] GameObject m_need_none= null;

    [Header("productions")]
    [SerializeField] GameObject m_prod_r0= null;
    [SerializeField] TextMeshProUGUI m_prod_r0_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r0_quantity= null;
    [SerializeField] GameObject m_prod_r1= null;
    [SerializeField] TextMeshProUGUI m_prod_r1_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r1_quantity= null;
    [SerializeField] GameObject m_prod_r2= null;
    [SerializeField] TextMeshProUGUI m_prod_r2_name= null;
    [SerializeField] TextMeshProUGUI m_prod_r2_quantity= null;
    [SerializeField] Toggle m_prod_sell_prod= null;
    [SerializeField] GameObject m_prod_none= null;

    public void DisplayItemInfos(string name, string desc, UIResourceData[] needs, UIResourceData[] prods, bool sellprod)
    {
        m_infos.SetActive(true);

        m_title.text = name;
        m_description.text = desc;

        if (needs != null)
        {
            if (needs.Length <= 0)
                m_need_none.SetActive(true);
            else m_need_none.SetActive(false);

            if (needs.Length > 0)
            {
                m_need_r0.SetActive(true);
                m_need_r0_name.text = needs[0].name;
                m_need_r0_quantity.text = needs[0].quantity.ToString();
            }
            else m_need_r0.SetActive(false);

            if (needs.Length > 1)
            {
                m_need_r1.SetActive(true);
                m_need_r1_name.text = needs[1].name;
                m_need_r1_quantity.text = needs[1].quantity.ToString();
            }
            else m_need_r1.SetActive(false);

            if (needs.Length > 2)
            {
                m_need_r2.SetActive(true);
                m_need_r2_name.text = needs[2].name;
                m_need_r2_quantity.text = needs[2].quantity.ToString();
            }
            else m_need_r2.SetActive(false);
        }
        else
        {
            m_need_r0.SetActive(false);
            m_need_r1.SetActive(false);
            m_need_r2.SetActive(false);

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
                m_prod_r0.SetActive(true);
                m_prod_r0_name.text = prods[0].name;
                m_prod_r0_quantity.text = prods[0].quantity.ToString();
            }
            else m_prod_r0.SetActive(false);

            if (prods.Length > 1)
            {
                m_prod_r1.SetActive(true);
                m_prod_r1_name.text = prods[1].name;
                m_prod_r1_quantity.text = prods[1].quantity.ToString();
            }
            else m_prod_r1.SetActive(false);

            if (prods.Length > 2)
            {
                m_prod_r2.SetActive(true);
                m_prod_r2_name.text = prods[2].name;
                m_prod_r2_quantity.text = prods[2].quantity.ToString();
            }
            else m_prod_r2.SetActive(false);
        }
        else
        {
            m_prod_r0.SetActive(false);
            m_prod_r1.SetActive(false);
            m_prod_r2.SetActive(false);

            m_prod_none.SetActive(true);
            m_prod_sell_prod.gameObject.SetActive(false);
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

        public UIResourceData(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }
    }

    public void SellProductionToogle()
    {
        if (SelectionManager.selection is Building)
        {
            ((Building)SelectionManager.selection).m_sellProduction = m_prod_sell_prod.isOn;
        }
    }
}
