using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportTruck : Vehicul
{
    protected bool m_goLoading = true;
    protected Goods m_Load;

    Goods.GoodsType m_targetResourceTypeForCollect;
    public Building mMasterBuilding;

    protected override void Start()
    {

    }
    public void AssignBuilding(Building b)
    {
        mMasterBuilding = b;
        if (m_goLoading)
        {
            /*if (b.m_sellProduction && b.HaveProductionInStock())
            {
                AssignPath(b.mCrtDoorNode.worldPosition, b);

            }
            else*/
            print("aa");
            if (b.DontHaveNeedInStock())
            {
                print("bb");
                StartCoroutine(FindResourceInABuilding(b));
            }
        }
    }
    
    protected IEnumerator FindResourceInABuilding(Building b)
    {
        bool find = false;
        while (!find)
        {
            for (int i = 0; i < b.mNeed.Length; i++)
            {
                //if (!b.mStock.ContainsKey(b.mNeed[i].type) || b.mStock[b.mNeed[i].type].quantity <= 0)
                {
                    Building target = GameManager.inst.FindNearestBuildingWithObjectInside(transform.position, b.mNeed[i].type);
                    if (target == null)
                        continue;

                    find = true;

                    target.mStock[b.mNeed[i].type].reserved += 1;

                    m_targetResourceTypeForCollect = b.mNeed[i].type;

                    AssignPath(target.mCrtDoorNode.worldPosition, target);
                    pathFinished = LoadToTarget;

                    break;
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    protected override void PathCompleted()
    {
        base.PathCompleted();
    }
    protected void LoadToTarget(Vehicul b)
    {
        m_Load = mBuilding.GetResources(m_targetResourceTypeForCollect);

        AssignPath(mMasterBuilding.mCrtDoorNode.worldPosition, mMasterBuilding);

        pathFinished = UnloadToMaster;
    }
    protected void UnloadToMaster(Vehicul b)
    {
        mBuilding.AddResources(m_Load);

        //m_Load = null;
        pathFinished = null;

        StartCoroutine(FindResourceInABuilding(mMasterBuilding));
    }
}
