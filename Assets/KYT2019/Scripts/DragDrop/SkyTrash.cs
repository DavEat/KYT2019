using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrash : VehiculTarget
{
    public int mNumberOfTrash = 6;
    public int mReservedTrash = 0;
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
        if (mNumberOfTrash > mReservedTrash)
        {
            if (v.mVehiculTarget != null)
                v.mVehiculTarget.UnAssignVehicul(v);

            v.ClearPathFinished();
            mReservedTrash++;

            v.mVehiculTarget = this;
            m_vehiculs.Add(v);
            SendVehiculHere(v);
        }
        else if (!m_vehiculs.Contains(v))
            FindTheOne(v);
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
        transform.localScale = Vector3.one * (((Mathf.Min(mNumberOfTrash, m_MaxNumberOfTrash) - 1) / (float)m_MaxNumberOfTrash) * .5f + .5f);

        if (mNumberOfTrash <= 0)
        {
            v.ClearPathFinished();
            GameManager.inst.RemoveSkyTrash(this);
            FindTheOne(v);

            Grid.inst.NodeFromWorldPoint(transform.position).buildable = true;
            Destroy(gameObject, .1f);
        }
        else
        {
            //find nearest sorter
            Building b = GameManager.FindNearestObject(GameManager.inst.mSorters, transform.position);
            v.AssignPath(b.mCrtDoorsNode[0].worldPosition, b);
            v.pathFinished += VehiculArrivedToDestination;
        }
    }
    protected override void VehiculArrivedToDestination(Vehicul v)
    {
        if (v.mBuilding != null)
        {
            mNumberOfTrash--;
            mReservedTrash--;
            v.mBuilding.AddResources(new Goods(m_production.type, 1));
            PoliticsManager.inst.DumpTreated();
        }

        v.pathFinished -= VehiculArrivedToDestination;

        if (mNumberOfTrash == 0)
        {
            mReservedTrash++;
            mNumberOfTrash = -1;
            SendVehiculHere(v);
        }
        else if (mNumberOfTrash > mReservedTrash)
        {
            mReservedTrash++;
            SendVehiculHere(v);
        }
        else FindTheOne(v);
    }

    protected SkyTrash FindTheOne(Vehicul v)
    {
        print("find the one - " + Time.time);
        SkyTrash s = GameManager.inst.FindNearestSkyTrash(transform.position);
        if (s != null)
            s.AssignVehicul(v);
        else v.AssignPath(GameManager.inst.tractor.transform.position);
        return s;
    }
}
