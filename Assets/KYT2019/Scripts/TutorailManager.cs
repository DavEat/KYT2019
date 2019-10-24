using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorailManager : MonoBehaviour
{
    public void ClearGridForEachChildOf(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Node n = Grid.inst.NodeFromWorldPoint(t.GetChild(i).position);
            n.buildable = true;
            n.walkable = true;
        }
    }
}
