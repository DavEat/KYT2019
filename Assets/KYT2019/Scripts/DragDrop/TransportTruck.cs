﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportTruck : Vehicul
{
    protected Goods m_Load;

    Goods.GoodsType m_targetResourceTypeForCollect;
    public Building mMasterBuilding;

    public void AssignBuilding(Building b)
    {
        mMasterBuilding = b;

        ClearPathFinished();

        if (b.DontHaveNeedInStock())
        {
            StartCoroutine(FindResourceInABuilding(b));
        }
    }
    
    protected IEnumerator FindResourceInABuilding(Building b)
    {
        bool find = false;

        while (!find)
        {
            for (int i = 0; i < b.mNeed.Length; i++)
            {
                if (!b.mStock.ContainsKey(b.mNeed[i].type) || b.mStock[b.mNeed[i].type].quantity <= 0)
                {
                    Building target = GameManager.inst.FindNearestBuildingWithObjectInside(transform.position, b.mNeed[i].type);
                    if (target == null)
                        continue;

                    find = true;

                    target.mStock[b.mNeed[i].type].reserved += 1;

                    m_targetResourceTypeForCollect = b.mNeed[i].type;

                    pathFinished = LoadToTarget;
                    AssignPath(target.mCrtDoorsNode[0].worldPosition, target);

                    break;
                }
            }
            if (!find)
            {
                int i = Random.Range(0, b.mNeed.Length);
                {
                    Building target = GameManager.inst.FindNearestBuildingWithObjectInside(transform.position, b.mNeed[i].type);
                    if (target != null)
                    {
                        find = true;

                        target.mStock[b.mNeed[i].type].reserved += 1;

                        m_targetResourceTypeForCollect = b.mNeed[i].type;

                        pathFinished = LoadToTarget;
                        AssignPath(target.mCrtDoorsNode[0].worldPosition, target);
                    }
                }
            }
            if (!find)
            {
                AssignPath(mMasterBuilding.mCrtDoorsNode[0].worldPosition);
            }
            yield return new WaitForSeconds(.5f);
        }
    }
    protected void LoadToTarget(Vehicul v)
    {
        if (mBuilding == null)
        {
            StartCoroutine(FindResourceInABuilding(mMasterBuilding));
            return;
        }
        
        m_Load = mBuilding.GetResources(m_targetResourceTypeForCollect);
        pathFinished = UnloadToMaster;
        AssignPath(mMasterBuilding.mCrtDoorsNode[0].worldPosition, mMasterBuilding);
    }
    protected void UnloadToMaster(Vehicul b)
    {
        mBuilding.AddResources(m_Load);

        //m_Load = null;
        pathFinished = null;

        StartCoroutine(FindResourceInABuilding(mMasterBuilding));
    }
}
