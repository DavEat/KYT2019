using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingTuto : Building
{
    public int step = 0;
    public UnityEvent locateEvent;
    public UnityEvent assignEvent;

    public override bool Locate(Node node, bool afterDrag = false)
    {
        bool b = base.Locate(node, afterDrag);

        if (step == 0 && b)
        {
            step++;
            locateEvent.Invoke();
        }

        return b;
    }

    public override bool DontHaveNeedInStock()
    {
        if (step == 1)
        {
            step++;
            assignEvent.Invoke();
        }

        return base.DontHaveNeedInStock();
    }
}
