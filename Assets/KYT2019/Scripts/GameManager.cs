using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform[] mExits;

    public List<Building> mSorters = new List<Building>();
    public List<Building> mBuildings = new List<Building>();
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

    internal Building FindLonguestWaitingBuilding()
    {
        Building bestObject = null;
        float maxTime = -2;
        foreach (Building o in mBuildings)
        {            
            if (o.mLastTimeResourceGetCollect > maxTime)
            {
                maxTime = o.mLastTimeResourceGetCollect;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal Building FindLonguestWaitingBuilding(Goods.GoodsType type)
    {
        Building bestObject = null;
        float maxTime = -2;
        foreach (Building o in mBuildings)
        {
            if (o is Sorter) continue;

            for (int i = 0; i < o.mNeed.Length; i++)
            {
                if (o.mNeed[i].type == type)
                    if (o.mLastTimeResourceReceive > maxTime)
                    {
                        maxTime = o.mLastTimeResourceReceive;
                        bestObject = o;
                    }
            }
        }
        return bestObject;
    }
}
