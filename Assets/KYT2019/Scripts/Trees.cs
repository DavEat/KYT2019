using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public bool mBuildable = false;

    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
        if (mBuildable)
            Grid.inst.NodeFromWorldPoint(transform.position).buildable = false;
        else Grid.inst.NodeFromWorldPoint(transform.position).walkable = false;
    }
}
