using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHelper_UnactiveOnHover : MonoBehaviour, IPointerEnterHandler 
{
    [SerializeField] GameObject m_object = null;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        m_object.SetActive(false);
    }
}
