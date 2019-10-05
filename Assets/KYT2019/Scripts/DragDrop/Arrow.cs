using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Singleton<Arrow>
{
    public void Assign(Transform parent, Vector3 localPosition)
    {
        gameObject.SetActive(true);
        transform.parent = parent;
        transform.localPosition = localPosition;
        transform.rotation = Quaternion.identity;
    }
    public void Unassign()
    {
        gameObject.SetActive(false);
    }
}
