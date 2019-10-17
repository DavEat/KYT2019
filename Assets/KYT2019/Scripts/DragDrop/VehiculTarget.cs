using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculTarget : Selectable
{
    #region Vars
    public List<Vehicul> m_vehiculs = new List<Vehicul>();
    #endregion
    #region MonoFunctions
    #endregion
    #region Functions
    public virtual void AssignVehicul(Vehicul v)
    {
        if (v.mVehiculTarget != null)
            v.mVehiculTarget.UnAssignVehicul(v);

        v.ClearPathFinished();

        v.mVehiculTarget = this;
        m_vehiculs.Add(v);
        SendVehiculHere(v);
    }
    public virtual void UnAssignVehicul(Vehicul v)
    {
        m_vehiculs.Remove(v);        
    }
    protected virtual void SendVehiculHere(Vehicul v)
    {
        v.pathFinished += VehiculArrivedHere;
        v.AssignPath(transform.position);
    }
    protected virtual void VehiculArrivedHere(Vehicul v)
    {
        //Debug.DrawRay(transform.position, Vector3.up * 10, Color.green, 10);
        //Debug.LogFormat("vehicul {0} arrived here", v.name, v.gameObject);
        v.pathFinished -= VehiculArrivedHere;
    }
    protected virtual void VehiculArrivedToDestination(Vehicul v)
    {
        //Debug.DrawRay(transform.position, Vector3.up, Color.red, 10);
        //Debug.LogFormat("vehicul {0} arrived to dst", v.name, v.gameObject);
        v.pathFinished -= VehiculArrivedToDestination;
        SendVehiculHere(v);
    }
    #endregion
}
