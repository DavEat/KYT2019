using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehiculDepotTuto : VehiculeDepot
{
    public int step = 0;
    public UnityEvent selectionEvent;

    public override void Selection()
    {
        base.Selection();

        if (step == 0)
        {
            step++;
            selectionEvent.Invoke();
        }
    }

    public override void Diselection()
    {
        base.Diselection();
    }
}
