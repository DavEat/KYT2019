using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyTrashTuto : SkyTrash
{
    public int step = 0;
    public UnityEvent assignVehcul;

    public SkyTrashTuto skyTrashTuto = null;

    public override void AssignVehicul(Vehicul v)
    {
        base.AssignVehicul(v);
        if (step == 0)
        {
            step++;
            skyTrashTuto.step++;
            skyTrashTuto.mNumberOfTrash = 10000;
            assignVehcul.Invoke();
        }
    }
}
