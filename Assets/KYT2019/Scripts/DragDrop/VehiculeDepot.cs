using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeDepot : Building
{

    public override void Selection()
    {
        SoundManager.inst.PlaySelection();

        for (int i = 0; i < mDoors.Length; i++)
            GameManager.inst.doorArrows[i].Assign(transform, new Vector3(mDoors[i].x + centerOffet.x, 0, mDoors[i].y + centerOffet.y) * m_nodeDiameter);
        GameManager.inst.selectionArrow.Assign(this);

        CanvasManager.inst.mVehiculeDepot.SetActive(true);
    }

    public override void Diselection()
    {
        for (int i = 0; i < GameManager.inst.doorArrows.Length; i++)
            GameManager.inst.doorArrows[i].Unsign();
        GameManager.inst.selectionArrow.Unsign();

        if (m_warning != null && !m_warning.activeSelf)
            BuildingPlaced();

        SoundManager.inst.PlayUnselection();

        CanvasManager.inst.mVehiculeDepot.SetActive(false);
    }
}
