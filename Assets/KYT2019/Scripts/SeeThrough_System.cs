using UnityEngine;

public class SeeThrough_System : MonoBehaviour
{
    [SerializeField] LayerMask m_AllLayer = 0;
    [SerializeField] Material m_material = null;
    //[SerializeField] Material m_material_black = null;
    [SerializeField] Material m_material_text = null;

    bool m_active;
    public bool active {
        set {
            m_active = value;
            if (!m_active)
            {
                m_material.SetVector("_MousePosition", Vector3.zero);
                //m_material_black.SetVector("_MousePosition", Vector3.zero);
                m_material_text.SetVector("_MousePosition", Vector3.zero);
            }
        }
    }

    Vector3 m_lastMousePosition;

    void Start()
    {
        //m_material = GetComponent<UnityEngine.UI.Image>().material;
    }

    void Update()
    {
        if (m_active)
        {
            if (m_lastMousePosition != Input.mousePosition)
            {
                m_lastMousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200, m_AllLayer))
                {
                    m_material.SetVector("_MousePosition", new Vector3(hit.point.x, transform.position.y, hit.point.z));
                    //m_material_black.SetVector("_MousePosition", new Vector3(hit.point.x, transform.position.y, hit.point.z));
                    m_material_text.SetVector("_MousePosition", new Vector3(hit.point.x, transform.position.y, hit.point.z));
                }
            }
        }
    }
}
