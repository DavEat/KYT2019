using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
        Grid.inst.NodeFromWorldPoint(transform.position).walkable = false;
    }
}
