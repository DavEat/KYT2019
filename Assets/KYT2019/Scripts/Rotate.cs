using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    #region Vars
    [SerializeField] Vector3 m_angle = Vector3.up;
    [SerializeField] float m_speed = 1;
    #endregion
    #region MonoFunctions
    void Update()
    {
        transform.Rotate(m_angle * m_speed * Time.deltaTime);
    }
    #endregion
}
