using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDst : MonoBehaviour
{
    public void Assign(Vector3 postion)
    {
        gameObject.SetActive(true);
        transform.position = postion;
    }
    public void Unsign()
    {
        gameObject.SetActive(false);
    }
}
