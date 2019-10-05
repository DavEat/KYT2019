using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDst : Singleton<ArrowDst>
{
    public void Assign(Vector3 postion)
    {
        gameObject.SetActive(true);
        transform.position = postion;
    }
    public void Unassign()
    {
        gameObject.SetActive(false);
    }
}
