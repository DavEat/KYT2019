using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void Assign(Transform parent, Vector3 localPosition)
    {
        gameObject.SetActive(true);
        transform.parent = parent;
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.identity;
    }
    public void Unsign()
    {
        gameObject.SetActive(false);
        transform.parent = null;
    }
}
