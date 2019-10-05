using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationLoop : MonoBehaviour
{
    #region Vars
    [Header("Only Y axis for now")]
    [SerializeField] Vector3 m_min = Vector3.zero;
    [SerializeField] Vector3 m_max = Vector3.up;
    [SerializeField] float m_speed = 1;
    bool m_up = true;
    #endregion
    #region MonoFunctions
    void Update()
    {
        if (m_up && transform.localPosition.y > m_max.y)
            m_up = false;
        else if (!m_up && transform.localPosition.y < m_min.y)
            m_up = true;

        transform.Translate((m_up ? m_max - m_min : m_min - m_max) * m_speed * Time.deltaTime);
        
    }
    #endregion
}
