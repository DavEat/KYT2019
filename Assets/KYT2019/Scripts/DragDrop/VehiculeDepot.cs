using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeDepot : Building
{
    public override void Selection()
    {
        SoundManager.inst.PlaySelection();

        Arrow.inst.Assign(transform, new Vector3(mDoor.x + centerOffet.x, 0, mDoor.y + centerOffet.y) * m_nodeDiameter);
        ArrowSel.inst.Assign(this);

        CanvasManager.inst.mVehiculeDepot.SetActive(true);
    }

    public override void Diselection()
    {
        Arrow.inst.Unsign();
        ArrowSel.inst.Unsign();

        if (m_warning != null && !m_warning.activeSelf)
            BuildingPlaced();

        SoundManager.inst.PlayUnselection();

        CanvasManager.inst.mVehiculeDepot.SetActive(false);
    }
}
