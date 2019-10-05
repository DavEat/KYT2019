using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform[] mExits;

    public List<Building> mSorters = new List<Building>();
    public List<SkyTrash> mSkyTrashs = new List<SkyTrash>();


    internal static Selectable FindNearestObject(List<Selectable> objects , Vector3 position)
    {
        Selectable bestObject = null;
        float minDst = 9999;
        foreach(Selectable o in objects)
        {
            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal static Building FindNearestObject(List<Building> objects, Vector3 position)
    {
        Building bestObject = null;
        float minDst = 9999;
        foreach (Building o in objects)
        {
            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal static SkyTrash FindNearestObject(List<SkyTrash> objects, Vector3 position)
    {
        SkyTrash bestObject = null;
        float minDst = 9999;
        foreach (SkyTrash o in objects)
        {
            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
}
