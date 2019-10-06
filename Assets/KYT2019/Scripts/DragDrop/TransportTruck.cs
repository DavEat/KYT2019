using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportTruck : Vehicul
{
    protected bool m_goLoading;
    protected Goods m_Load;

    protected override void Start()
    {
        m_goLoading = true;
        //Add check if building have object to load
        Building b = GameManager.inst.FindLonguestWaitingBuilding();
        //if (b != null)
        AssignPath(b.mCrtDoorNode.worldPosition, b);
    }
    protected override void PathCompleted()
    {
        print("pathfinish");
        m_moving = false;
        ArrowDst.inst.Unassign();
        
        if (m_goLoading)
        {
            m_goLoading = false;
            m_Load = mBuilding.GetResources();

            Building b = GameManager.inst.FindLonguestWaitingBuilding(m_Load.type);
            AssignPath(b.mCrtDoorNode.worldPosition, b);
        }
        else
        {
            mBuilding.AddResources(m_Load);

            m_goLoading = true;
            Building b = GameManager.inst.FindLonguestWaitingBuilding();
            AssignPath(b.mCrtDoorNode.worldPosition, b);
        }
    }
}
