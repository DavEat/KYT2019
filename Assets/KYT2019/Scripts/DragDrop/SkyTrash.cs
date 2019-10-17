using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrash : VehiculTarget
{
    public int mNumberOfTrash = 6;
    int m_MaxNumberOfTrash;

    [SerializeField] Goods m_production = null;

    protected override void Start()
    {
        //base.Start();
        /*Node n = Grid.inst.NodeFromWorldPoint(transform.position);
        if (n != null)
            n.walkable = false;*/

        m_MaxNumberOfTrash = mNumberOfTrash;

        GameManager.inst.mSkyTrashs.Add(this);

        transform.localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);

        Grid.inst.NodeFromWorldPoint(transform.position).buildable = false;
    }
    public override void AssignVehicul(Vehicul v)
    {
        if (m_vehiculs.Count <= 0)
        {
            if (v.mVehiculTarget != null)
                v.mVehiculTarget.UnAssignVehicul(v);

            v.ClearPathFinished();

            v.mVehiculTarget = this;
            m_vehiculs.Add(v);
            SendVehiculHere(v);
        }
        else if (m_vehiculs[0] != v)
        {
            SkyTrash s = GameManager.inst.FindNearestSkyTrash(transform.position, null);
            if (s != null)
                s.AssignVehicul(v);
        }
    }
    protected override void VehiculArrivedHere(Vehicul v)
    {
        base.VehiculArrivedHere(v);

        StartCoroutine(Wait_AfterVehiculArrivedHere(v, .2f));
        //VehiculArrivedHereLogic(v);
    }
    IEnumerator Wait_AfterVehiculArrivedHere(Vehicul v, float time)
    {
        yield return new WaitForSeconds(time);

        VehiculArrivedHereLogic(v);
    }
    void VehiculArrivedHereLogic(Vehicul v)
    {
        transform.localScale = Vector3.one * (((mNumberOfTrash - 1) / (float)m_MaxNumberOfTrash) * .5f + .5f);

        if (mNumberOfTrash <= 0)
        {
            v.ClearPathFinished();
            GameManager.inst.RemoveSkyTrash(this);
            SkyTrash s = GameManager.inst.FindNearestSkyTrash(transform.position, null);
            if (s != null)
                s.AssignVehicul(v);

            Grid.inst.NodeFromWorldPoint(transform.position).buildable = true;
            Destroy(gameObject, .1f);
        }
        else
        {
            //find nearest sorter
            Building b = GameManager.FindNearestObject(GameManager.inst.mSorters, transform.position);
            v.AssignPath(b.mCrtDoorNode.worldPosition, b);
            v.pathFinished += VehiculArrivedToDestination;
        }
    }
    protected override void VehiculArrivedToDestination(Vehicul v)
    {
        if (v.mBuilding != null)
        {
            mNumberOfTrash--;
            v.mBuilding.AddResources(new Goods(m_production.type, 1));
            PoliticsManager.inst.DumpTreated();
        }

        base.VehiculArrivedToDestination(v);
    }
}
