using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrash : VehiculTarget
{
    public int mNumberOfTrash = 6;
    int m_MaxNumberOfTrash;

    protected override void Start()
    {
        base.Start();
        /*Node n = Grid.inst.NodeFromWorldPoint(transform.position);
        if (n != null)
            n.walkable = false;*/

        m_MaxNumberOfTrash = mNumberOfTrash;

        GameManager.inst.mSkyTrashs.Add(this);

        transform.localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
    }
    protected override void VehiculArrivedHere(Vehicul v)
    {
        base.VehiculArrivedHere(v);

        StartCoroutine(Wait_AfterVehiculArrivedHere(v, .6f));
    }
    IEnumerator Wait_AfterVehiculArrivedHere(Vehicul v, float time)
    {
        yield return new WaitForSeconds(time);

        transform.localScale = Vector3.one * (((mNumberOfTrash - 1) / (float)m_MaxNumberOfTrash) * .5f + .5f);

        if (mNumberOfTrash <= 0)
        {
            GameManager.inst.mSkyTrashs.Remove(this);
            SkyTrash s = GameManager.FindNearestObject(GameManager.inst.mSkyTrashs, transform.position);
            s.AssignVehicul(v);
            Destroy(gameObject, .1f);
        }
        else
        {
            //find nearest sorter
            Building b = GameManager.FindNearestObject(GameManager.inst.mSorters, transform.position);
            v.AssignPath(b.mCrtDoorNode.worldPosition);
            v.pathFinished += VehiculArrivedToDestination;
        }
    }
    protected override void VehiculArrivedToDestination(Vehicul v)
    {
        mNumberOfTrash--;

        base.VehiculArrivedToDestination(v);
    }
}
